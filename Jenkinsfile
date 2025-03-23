pipeline {
    agent any

    environment {
        DOCKER_BUILDKIT = '1'
        COMPOSE_DOCKER_CLI_BUILD = '1'
        MYSQL_ROOT_PASSWORD = credentials('yes-location-mysql-root-password')
        MYSQL_DATABASE = 'yes_location'
        MYSQL_USER = 'yes_location'
        MYSQL_PASSWORD = credentials('yes-location-mysql-user-password')
    }

    stages {
        stage('Check Environment') {
      steps {
        sh '''
                    echo "Checking Docker installation..."
                    docker --version
                    docker-compose --version

                    echo "Checking Docker permissions..."
                    docker ps
                '''
      }
        }

        stage('Checkout') {
      steps {
        checkout scm
      }
        }

        stage('Build') {
      steps {
        sh '''
                    cd backend
                    dotnet restore
                    dotnet build -c Release
                    dotnet publish -c Release -o publish
                '''
      }
        }

        stage('Test') {
      steps {
        sh '''
                    cd backend
                    dotnet test
                '''
      }
        }

        stage('Deploy') {
      when {
        branch 'main'
      }
      steps {
        sh '''
                    echo "Setting up deployment environment..."
                    chmod +x deploy.sh

                    echo "Starting deployment..."
                    ./deploy.sh

                    echo "Verifying deployment..."
                    docker ps | grep yes_location
                '''
      }
        }
    }

    post {
        always {
      cleanWs()
        }
        success {
      echo 'Déploiement réussi!'
        }
        failure {
      echo 'Déploiement échoué!'
        }
    }
}
