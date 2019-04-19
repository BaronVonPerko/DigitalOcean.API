﻿using System.Collections.Generic;
using DigitalOcean.API.Clients;
using DigitalOcean.API.Http;
using DigitalOcean.API.Models.Responses;
using NSubstitute;
using RestSharp;
using Xunit;

namespace DigitalOcean.API.Tests.Clients {
    public class TagsClientTest {
        [Fact]
        public void CorrectRequestForGet() {
            var factory = Substitute.For<IConnection>();
            var client = new TagsClient(factory);

            client.Get("notarealtag");

            var parameters = Arg.Is<List<Parameter>>(list => (string)list[0].Value == "notarealtag");
            factory.Received().ExecuteRequest<Tag>("tags/{name}", parameters, null, "tag");
        }

        [Fact]
        public void CorrectRequestForGetAll() {
            var factory = Substitute.For<IConnection>();
            var client = new TagsClient(factory);

            client.GetAll();

            factory.Received().GetPaginated<Tag>("tags", null, "tags");
        }

        [Fact]
        public void CorrectRequestForCreate() {
            var factory = Substitute.For<IConnection>();
            var client = new TagsClient(factory);

            client.Create("notarealtag");

            factory.Received().ExecuteRequest<Tag>("tags", null, Arg.Is<Models.Requests.Tag>(data => data.Name == "notarealtag"), "tag", Method.POST);
        }

        [Fact]
        public void CorrectRequestForDelete() {
            var factory = Substitute.For<IConnection>();
            var client = new TagsClient(factory);

            client.Delete("notarealtag");

            var parameters = Arg.Is<List<Parameter>>(list => (string)list[0].Value == "notarealtag");
            factory.Received().ExecuteRaw("tags/{name}", parameters, null, Method.DELETE);
        }

        [Fact]
        public void CorrectRequestForTag() {
            var factory = Substitute.For<IConnection>();
            var client = new TagsClient(factory);

            List<KeyValuePair<string, string>> resources = new List<KeyValuePair<string, string>>(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("9001", "droplet"),
                new KeyValuePair<string, string>("9002", "droplet")
            });

            client.Tag("notarealtag", resources);

            var parameters = Arg.Is<List<Parameter>>(list => (string)list[0].Value == "notarealtag");
            factory.Received().ExecuteRaw("tags/{name}/resources", parameters, Arg.Is<Models.Requests.TagResource>(data => data.Resources[1].Id == "9002"), Method.POST);
        }

        [Fact]
        public void CorrectRequestForUntag() {
            var factory = Substitute.For<IConnection>();
            var client = new TagsClient(factory);

            List<KeyValuePair<string, string>> resources = new List<KeyValuePair<string, string>>(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("9001", "droplet"),
                new KeyValuePair<string, string>("9002", "droplet")
            });

            client.Untag("notarealtag", resources);

            var parameters = Arg.Is<List<Parameter>>(list => (string)list[0].Value == "notarealtag");
            factory.Received().ExecuteRaw("tags/{name}/resources", parameters, Arg.Is<Models.Requests.TagResource>(data => data.Resources[1].Id == "9002"), Method.DELETE);
        }
    }
}
