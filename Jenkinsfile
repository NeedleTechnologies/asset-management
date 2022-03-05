pipeline {
    agent any 
    stages {
        stage('Publishing'){
            steps {
                sh "sudo cp -r /var/lib/jenkins/workspace/Assets/deployment/* /var/www/assets/"
            }
        }

        stage('Change Directory'){
            steps{
                sh "cd /var/lib/jenkins/workspace/Assets/"
            }
        }
        stage('Install Depenpencies'){
            steps{
                sh "sudo dotnet restore"
            }
        }
        // stage('Run Migration'){
        //     steps{
        //         sh "sudo dotnet ef database update"
        //     }
        // }
        stage('Restart Service'){
            steps{
                sh 'sudo systemctl restart assets-management'
            }
        }
        stage('Restart MySQL Service'){
            steps{
                sh 'sudo systemctl restart mysql'
            }
        }
    }
}