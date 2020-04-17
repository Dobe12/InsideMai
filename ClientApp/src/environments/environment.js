"use strict";
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
Object.defineProperty(exports, "__esModule", { value: true });
exports.environment = {
    production: false,
    api_url: 'https://localhost:44350/api/',
    // Levels
    MaiLvl: 1,
    UniversityLvl: 2,
    DepartmentLvl: 3,
    GroupLvl: 4,
    // PostTypes
    AllPosts: 1,
    Question: 2,
    Article: 3,
    Advert: 4,
    Event: 5
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
//# sourceMappingURL=environment.js.map