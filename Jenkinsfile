pipeline {
  environment {
    dockerimagename = "ktei8htop15122004/savingaccount_be-sa-api"
    dockerImage = ""
    DOCKERHUB_CREDENTIALS = credentials('dockerhub')
  }

  agent {
    kubernetes {
      yaml '''
      apiVersion: v1
      kind: Pod
      spec:
        serviceAccountName: jenkins-admin
        dnsConfig:
          nameservers:
            - 8.8.8.8
        containers:
        - name: docker
          image: docker:latest
          imagePullSecrets:
            - name: regcred
          command:
            - cat
          tty: true
          securityContext:
            privileged: true
          volumeMounts:
            - mountPath: /var/run/docker.sock
              name: docker-sock
        - name: kubectl
          image: bitnami/kubectl:latest
          imagePullSecrets:
            - name: regcred
          command:
            - cat
          securityContext:
            runAsUser: 0
          tty: true
        volumes:
          - name: docker-sock
            hostPath:
              path: /var/run/docker.sock
      '''
    }
  }

  stages {
    stage('Unit Test') {
      when {
        expression {
          return env.BRANCH_NAME != 'master';
        }
      }
      steps {
        sh 'echo Unit Test'
      }
    }

    stage('Build image') {
      steps {
        container('docker') {
          script {
            sh 'docker build --network=host -t ktei8htop15122004/savingaccount_be-sa-api .'
          }
        }
      }
    }

    // stage('Pushing Image') {
    //   steps {
    //     container('docker') {
    //       script {
    //         sh 'echo $DOCKERHUB_CREDENTIALS_PSW | docker login -u $DOCKERHUB_CREDENTIALS_USR --password-stdin'
    //         sh 'docker tag ktei8htop15122004/savingaccount_be-sa-api ktei8htop15122004/savingaccount_be-sa-api'
    //         sh 'docker push ktei8htop15122004/savingaccount_be-sa-api:latest'
    //       }
    //     }
    //   }
    // }
    stage('Create SQL Deployment , PVC and Service YAML') {
  steps {
    writeFile file: '/home/jenkins/agent/workspace/SavingAccountBE_main/deployment-sql.yaml', text: '''apiVersion: apps/v1
kind: Deployment
metadata:
  labels:
    io.kompose.service: sqlserver
  name: sqlserver
spec:
  replicas: 1
  selector:
    matchLabels:
      io.kompose.service: sqlserver
  strategy:
    type: Recreate
  template:
    metadata:
      labels:
        io.kompose.service: sqlserver
    spec:
      containers:
        - name: sqlserver
          image: mcr.microsoft.com/mssql/server:2019-latest
          securityContext:
            runAsUser: 0 
          env:
            - name: ACCEPT_EULA
              value: "Y"
            - name: MSSQL_SA_PASSWORD
              value: 1236fG543$
          ports:
            - containerPort: 1433
              protocol: TCP
          volumeMounts:
            - mountPath: /var/opt/mssql
              name: sql-data
          resources:
            limits:
              memory: "5Gi"
              cpu: "1"
            requests:
              memory: "4Gi"
              cpu: "1"
      restartPolicy: Always
      volumes:
        - name: sql-data
          persistentVolumeClaim:
            claimName: sql-data
---
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  labels:
    io.kompose.service: sql-data
  name: sql-data
spec:
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 100Mi
---
apiVersion: v1
kind: Service
metadata:
  labels:
    io.kompose.service: sqlserver
  name: sqlserver
spec:
  ports:
    - name: "sqlserver"
      port: 1433
      targetPort: 1433
  selector:
    io.kompose.service: sqlserver

'''
  }
}

    stage('Create Deployment and Service YAML for BE') {
  steps {
    writeFile file: '/home/jenkins/agent/workspace/SavingAccountBE_main/deployment-be.yaml', text: '''apiVersion: apps/v1
kind: Deployment
metadata:
  labels:
    io.kompose.service: sa-api
  name: sa-api
spec:
  replicas: 1
  selector:
    matchLabels:
      io.kompose.service: sa-api
  template:
    metadata:
      labels:
        io.kompose.service: sa-api
    spec:
      containers:
        - args:
            - dotnet
            - ef
            - database update
          env:
            - name: ASPNETCORE_ENVIRONMENT
              value: Development
            - name: ASPNETCORE_URLS
              value: http://+:80
            - name: DATABASE_SETTINGS_USERS_DATABASE
              value: Server=sqlserver,1434;Database=User;User Id=sa;Password=1236fG543$;TrustServerCertificate=true
          image: ktei8htop15122004/savingaccount_be-sa-api
          name: savingaccount-be
          ports:
            - containerPort: 80
              protocol: TCP
          resources:
            limits:
              memory: "512Mi"
              cpu: "500m"
            requests:
              memory: "256Mi"
              cpu: "250m"
      restartPolicy: Always
---
apiVersion: v1
kind: Service
metadata:
  labels:
    io.kompose.service: sa-api
  name: sa-api
spec:
  ports:
    - name: "8080"
      port: 8080
      targetPort: 80
  selector:
    io.kompose.service: sa-api
'''
  }
}


    stage('Deploying App to Kubernetes') {
      steps {
        container('kubectl') {
          withCredentials([file(credentialsId: 'kube-config-admin', variable: 'TMPKUBECONFIG')]) {
            sh "cat \$TMPKUBECONFIG"
            sh "cp \$TMPKUBECONFIG ~/.kube/config"
            sh "ls -l \$TMPKUBECONFIG"
            sh "pwd"
            sh "kubectl apply -f deployment-sql.yaml"
            sh "kubectl apply -f deployment-be.yaml"
          }
        }
      }
    }
  }
}
