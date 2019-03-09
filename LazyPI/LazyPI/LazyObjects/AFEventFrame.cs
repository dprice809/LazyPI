﻿using LazyPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LazyPI.LazyObjects
{
    public class AFEventFrames : ObservableCollection<AFEventFrame>
    {
        public AFEventFrame this[string Name]
        {
            get
            {
                return this.SingleOrDefault(x => x.Name == Name);
            }
        }

        internal AFEventFrames(IEnumerable<AFEventFrame> frames)
            : base(frames)
        {
        }

        protected override void InsertItem(int index, AFEventFrame item)
        {
            item.IsNew = true;
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            AFEventFrame frame = this[index];
            frame.Delete();
            frame.CheckIn();
            base.RemoveItem(index);
        }
    }

    public class AFEventFrame : BaseObject
    {
        private DateTime _StartTime;
        private DateTime _EndTime;
        private AFElementTemplate _Template;
        private AFEventFrames _EventFrames;
        private AFAttributes _Attributes;
        private ObservableCollection<string> _CategoryNames;
        private static IAFEventFrameController _EventFrameController;

        #region "Properties"

        public string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
                _IsDirty = true;
            }
        }

        public string Description
        {
            get
            {
                return base.Description;
            }
            set
            {
                base.Description = value;
                _IsDirty = true;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
                _IsDirty = true;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                _EndTime = value;
                _IsDirty = true;
            }
        }

        public AFElementTemplate Template
        {
            get
            {
                if (_Template == null)
                {
                    var templateName = _EventFrameController.GetEventFrameTemplate(_Connection, _WebID);
                    _Template = AFElementTemplate.FindByName(templateName);
                }

                return _Template;
            }
        }

        public AFAttributes Attributes
        {
            get
            {
                if (_Attributes == null)
                {
                    var attrs = _EventFrameController.GetAttributes(_Connection, _WebID, "*", "*", "*", "*", false, "Name", "Ascending", 0, false, false, 1000);
                    _Attributes = new AFAttributes(attrs.ToList());
                    _Attributes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ItemsChangedMethod);
                }

                return _Attributes;
            }
            set
            {
                _Attributes = value;
                _IsDirty = true;
            }
        }

        public AFEventFrames EventFrames
        {
            get
            {
                if (_EventFrames == null)
                {
                    _EventFrames = new AFEventFrames(_EventFrameController.GetEventFrames(_Connection, _WebID));
                    _EventFrames.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ItemsChangedMethod);
                }

                return _EventFrames;
            }
            set
            {
                _EventFrames = value;
                _IsDirty = true;
            }
        }

        public IEnumerable<string> CategoryNames
        {
            get
            {
                return _CategoryNames;
            }
        }

        #endregion "Properties"

        #region "Constructors"

        public AFEventFrame()
        {
        }

        internal AFEventFrame(Connection Connection, string WebID, string ID, string Name, string Description, string Path)
            : base(Connection, WebID, ID, Name, Description, Path)
        {
            Initialize();
        }

        private void Initialize()
        {
            _EventFrameController = GetController(_Connection);
        }

        private static IAFEventFrameController GetController(Connection Connection)
        {
            IAFEventFrameController result = null;

            if (Connection is WebAPI.WebAPIConnection)
            {
                result = new WebAPI.AFEventFrameController();
            }

            return result;
        }

        #endregion "Constructors"

        #region "Static Methods"

        public static AFEventFrame Find(Connection Connection, string ID)
        {
            return GetController(Connection).Find(Connection, ID);
        }

        public static AFEventFrame FindByPath(Connection Connection, string Path)
        {
            return GetController(Connection).FindByPath(Connection, Path);
        }

        #endregion "Static Methods"

        #region "Public Methods"

        public void CheckIn()
        {
            if (_IsDirty && !_IsDeleted)
            {
                _EventFrameController.Update(_Connection, this);

                if (_EventFrames != null)
                {
                    foreach (AFEventFrame frame in _EventFrames.Where(x => x.IsNew || x.IsDeleted))
                    {
                        if (frame.IsNew)
                            _EventFrameController.CreateEventFrame(_Connection, _WebID, frame);
                        else if (frame.IsDeleted)
                        {
                            frame.Delete();
                            frame.CheckIn();
                        }
                    }
                }

                if (_Attributes != null)
                {
                    foreach (AFAttribute attr in _Attributes.Where(x => x.IsDeleted))
                    {
                        AFAttribute.Delete(_Connection, attr.WebID);
                    }
                }
            }

            ResetState();
        }

        /// <summary>
        /// Sets the
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            _EventFrameController.Delete(_Connection, _WebID);
            _IsDeleted = true;
        }

        #endregion "Public Methods"

        private void ResetState()
        {
            _IsNew = false;
            _IsDirty = false;
            _EventFrames = null;
            _Attributes = null;
            _Template = null;
            _CategoryNames = null;
        }
    }
}