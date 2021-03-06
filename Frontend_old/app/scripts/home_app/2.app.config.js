(function () {

    'use strict';

    angular.module('tesi.homeApp')
    /*.constant('env_config', {
      apiURL: 'http://159.149.142.33:4567',
      DCNConverterURL : 'http://159.149.142.35:5051'
    });*/
    .constant('env_config', {
      // apiURL: 'http://localhost:4567',
      // DCNConverterURL : 'http://159.149.142.35:5051',
      // WSURL: 'ws://localhost:4568/websocket/echo',
      // WSURL_UNIMI: 'ws://159.149.142.35:5053'
      apiURL: 'http://localhost:4567',
      DCNConverterURL : 'http://localhost:5051',
      WSURL: 'ws://localhost:4568/websocket/echo',
      WSURL_UNIMI: 'ws://localhost:5052'
    });

})();
