﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace LazyPI.WebAPI
{
    public class AFDatabaseController : RestRequester<ResponseModels.AFDB>, LazyPI.LazyObjects.IAFDatabaseController
    {
        public LazyPI.LazyObjects.AFDatabase Find(LazyPI.Common.Connection Connection, string ID)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var endpoint = "/assetdatabases/{webId}";
            var data = base.Read(webConnection, endpoint, ID);

            return new LazyObjects.AFDatabase(Connection, data.WebId, data.Id, data.Name, data.Description, data.Path);
        }

        public LazyObjects.AFDatabase FindByPath(LazyPI.Common.Connection Connection, string Path)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var endpoint = "/assetdatabases";
            var data = base.ReadByPath(webConnection, endpoint, Path);

            return new LazyObjects.AFDatabase(Connection, data.WebId, data.Id, data.Name, data.Description, data.Path);
        }

        public bool Update(LazyPI.Common.Connection Connection, LazyPI.LazyObjects.AFDatabase AFDB)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var endpoint = "/attributes/{webId}";
            ResponseModels.AFDB body = DataConversions.Convert(AFDB);

            return base.Update(webConnection, endpoint, body);
        }

        public bool Delete(LazyPI.Common.Connection Connection, string DatabaseID)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var endpoint = "/attributes/{webId}";

            return base.Delete(webConnection, endpoint, DatabaseID);
        }

        public bool CreateElement(LazyPI.Common.Connection Connection, string DatabaseID, LazyPI.LazyObjects.AFElement Element)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var request = new RestRequest("/assetdatabases/{webId}/elements", Method.POST);
            request.AddUrlSegment("webId", DatabaseID);
            ResponseModels.AFElement body = DataConversions.Convert(Element);
            request.AddParameter("application/json; charset=utf-8", Newtonsoft.Json.JsonConvert.SerializeObject(body), ParameterType.RequestBody);

            var statusCode = webConnection.Client.Execute(request).StatusCode;

            return ((int)statusCode == 204);
        }

        public bool CreateEventFrame(LazyPI.Common.Connection Connection, string DatabaseID, LazyPI.LazyObjects.AFEventFrame EventFrame)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var request = new RestRequest("/assetdatabases/{webId}/eventframes", Method.POST);
            request.AddUrlSegment("webId", DatabaseID);
            ResponseModels.AFEventFrame body = DataConversions.Convert(EventFrame);
            request.AddParameter("application/json; charset=utf-8", Newtonsoft.Json.JsonConvert.SerializeObject(body), ParameterType.RequestBody);

            var statusCode = webConnection.Client.Execute(request).StatusCode;

            return ((int)statusCode == 204);
        }

        public IEnumerable<LazyPI.LazyObjects.AFElement> GetElements(LazyPI.Common.Connection Connection, string DatabaseID)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var request = new RestRequest("/assetdatabases/{webId}/elements");
            request.AddUrlSegment("webId", DatabaseID);

            var response = webConnection.Client.Execute<ResponseModels.ResponseList<ResponseModels.AFElement>>(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error searching for elements. (See Inner Details)", response.ErrorException);
            }

            List<LazyObjects.AFElement> results = new List<LazyObjects.AFElement>();

            foreach (var element in response.Data.Items)
            {
                results.Add(new LazyObjects.AFElement(Connection, element.WebId, element.Id, element.Name, element.Description, element.Path));
            }

            return results;
        }

        public IEnumerable<LazyPI.LazyObjects.AFEventFrame> GetEventFrames(LazyPI.Common.Connection Connection, string DatabaseID)
        {
            WebAPIConnection webConnection = (WebAPIConnection)Connection;
            var request = new RestRequest("/assetdatabases/{webId}/eventframes");
            request.AddUrlSegment("webId", DatabaseID);

            var response = webConnection.Client.Execute<ResponseModels.ResponseList<ResponseModels.AFEventFrame>>(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error searching for eventframes. (See Inner Details)", response.ErrorException);
            }

            List<LazyObjects.AFEventFrame> results = new List<LazyObjects.AFEventFrame>();

            foreach (var eventframe in response.Data.Items)
            {
                results.Add(new LazyObjects.AFEventFrame(Connection, eventframe.WebId, eventframe.Id, eventframe.Name, eventframe.Description, eventframe.Path));
            }

            return results;
        }
    }
}
