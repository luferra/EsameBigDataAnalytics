'use strict';

angular.module('tesi.indexApp')
  .controller('PasswordRecoveryCtrl', function ($rootScope, $scope, $window, $routeParams, recoveryService, alertsManager) {
	  
      $scope.alerts = alertsManager.alerts;
      
      $scope.sendRecoveryEmail = function(){
		recoveryService.sendRecoveryEmail($scope.emailAddress).then(
		  function(response){		
			if(response.content.emailSent==true){
                alertsManager.clearAlerts();
                alertsManager.addAlert("If your email is associated with an account, you will receive an email with the recovery procedure", 'alert-success');			    
			}
		},
		function(error){            	
			alertsManager.clearAlerts();
			alertsManager.addAlert("If your email is associated with an account, you will receive an email with the recovery procedure", 'alert-success');            
		});
      };     

  });

