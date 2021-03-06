(function () {

    'use strict';

    angular.module('tesi.indexApp').controller('SignupCtrl', SignupCtrl);

    function SignupCtrl($scope, signupService, $location, alertsManager) {

        alertsManager.clearAlerts();
		$scope.alerts = alertsManager.alerts;
		
        $scope.signup = function(){
            signupService.signup($scope.user)
                .then(
                function(response){         
                    alertsManager.clearAlerts();
                    var msg = $scope.user.name + ' you registered successfully, you can now proceed with login';                    
                    alertsManager.addAlert(msg, 'alert-success');
                    $location.path('#/login');
                },
                function(error){
                    //TODO Show alert
                    alertsManager.clearAlerts();
                    alertsManager.addAlert(error.content.message, 'alert-danger');
                }
            );
        };

    }


})();
