"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var environment_1 = require("../../../environments/environment");
var DataService = /** @class */ (function () {
    function DataService(controllerName, http) {
        this.controllerName = controllerName;
        this.http = http;
        this.url = environment_1.environment.api_url + this.controllerName;
    }
    DataService.prototype.getAll = function () {
        return this.http.get(this.url);
    };
    DataService.prototype.get = function (id) {
        return this.http.get(this.url + '/' + id);
    };
    DataService.prototype.create = function (resource) {
        return this.http.post(this.url, JSON.stringify(resource));
    };
    DataService.prototype.update = function (resource) {
        return this.http.patch(this.url + '/' + resource.id, JSON.stringify(resource));
    };
    DataService.prototype.delete = function (resource) {
        return this.http.delete(this.url + '/' + resource.id);
    };
    return DataService;
}());
exports.DataService = DataService;
//# sourceMappingURL=data.service.js.map