pipeline {
    agent any

    environment {
        DOCKER_BUILDKIT = '0'
        COMPOSE_DOCKER_CLI_BUILD = '0'
        MYSQL_ROOT_PASSWORD = credentials('yes-location-mysql-root-password')
        MYSQL_DATABASE = 'yes_location'
        MYSQL_USER = 'yes_location'
        MYSQL_PASSWORD = credentials('yes-location-mysql-user-password')
        JWT_TOKEN_KEY = credentials('yes-locationjwt-token-key')
        JWT_ISSUER = 'yes-location'
        JWT_AUDIENCE = 'yes-location'
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

        stage('Build and Test') {
      steps {
        script {
          // Créer le fichier appsettings.json temporaire pour les tests
          writeFile file: 'backend/YesLocation.Api/appsettings.json', text: """
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "TokenKey": "${JWT_TOKEN_KEY}",
    "Issuer": "${JWT_ISSUER}",
    "Audience": "${JWT_AUDIENCE}"
  }
}
"""
        }

        sh '''
                    # Construire l'image de développement
                    DOCKER_BUILDKIT=0 docker build -f backend/Dockerfile.dev -t yes-location-dev ./backend

                    # Exécuter la compilation et les tests dans le conteneur
                    docker run --rm -v "${WORKSPACE}/backend:/app" yes-location-dev bash -c "dotnet restore && dotnet build -c Release && dotnet test"
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
