# DotNet template legacy project
Este proyecto está basado en el [Project Template](https://docs.gitlab.com/ee/gitlab-basics/create-project.html) de Gitlab, y fue modificado por el equipo de DevOps, para adaptar una Pipeline de CI/CD standard al mismo. Ante cualquier duda o sugerencia, por favor contectarnos a través del canal de Slack **#gitlab-legacy**.

### CI Pipeline
Actualmente la Pipeline para dotnet cuenta de 3 pasos estándar, traídos de los [Global CI Templates](https://gitlab.ar.bsch/santander-tecnologia/global-ci-templates):
  - package
  - db-migrate  
  - quality
  - pre-deploy
  - deploy 


##### Package
Ejecuta: 
* `nuget.exe restore ap.sln`, para recuperar la configuracion del solution. 
* De acuerdo a la version del ide, ejecuta el build de la aplicacion con todos los parametros `msbuild.exe ap.sln <parametros>"`, esto dejara los binarios en la carpeta 'Deploy'. 

Ver [este repositorio](https://gitlab.ar.bsch/santander-tecnologia/global-ci-templates/-/blob/latest/net/.build-package.yml)


##### DB-Migrate
Este stage ejecuta flyway en cada ambiente tomando los archicos de configuracion del $CONF_PATH. Dentro del Conf path, debe haber un archivo llamado fw.conf con toda la configuracion necesaria. Para mayor informacion, ver el siguiente instructivo https://confluence.ar.bsch/display/DEV/Flyway y https://gitlab.ar.bsch/santander-tecnologia/project-templates/flyway-template/-/blob/master/README.md
Variables a agregar en el ci-variables:
* USE_FLYWAY: "true" ( Colocar TRUE si se desa utulizar el deploy de Flyway y FALSE si no lo desea. )
* CONF_PATH: "db/conf" (Carpeta obligatoria para ejecutar los archivos de configuración.)
* SQL_PATH: "db/sql" (carpeta obligatoria para ejecutar los scripts sql. Habrá una por ambiente para que puedan separa aquellos scripts según correspondan.)


##### Quality
Utiliza [SonarQube](http://sonartest.ar.bsch:9000) para analizar Code Smells, Vulnerabilidades, Coverage de los Tests, etc. Se puede ver el resultado del último Quality Gate, si se desea. Para realizar esto, por favor comunicarse con el equipo de DevOps, ya que requiere configuración de Administrador de Sonar. Para más información respecto a Sonar, recomendamos visitar su [documentación](http://sonartest.ar.bsch:9000/documentation). Tener en cuenta, de configurar la variable "HAVE_TEST". El template completo puede verse en [este repositorio](https://gitlab.ar.bsch/santander-tecnologia/global-ci-templates/-/blob/latest/net/.sonar-scanner.yml).


##### Pre-Deploy
Este stage lo que hara es generar dinamicamente un excecute.yml. Dicho archivo contendra tantos jobs de deploys como servers hayan sido definidos en las variables "SERVERS_DEV,SERVERS_STG,SERVERS_PROD", tal como se detalla a continuacion. Tener en cuenta de agregar en el .ci-variables con las variables y reemplazar el contenido con los nombres de los runners segun ambiente.
* SERVERS_DEV: '<tag-runner-1>;<tag-runner-2>;<tag-runner-N>'
* SERVERS_STG: '<tag-runner-1>;<tag-runner-2>;<tag-runner-N>'
* SERVERS_PROD: '<tag-runner-1>;<tag-runner-2>;<tag-runner-N>'

##### Deploy
Este step, lo que hará es tomar el deploy-<environment>.json de acuardo al branch en el que se esta corriendo y recorrer cada uno de los aplications. Esto chequeara si el hostname coincide con el del servidor, detendra el webappool, comenzará a copiar tantos los binarios como los archivos de configuracion en las rutas indicadas y por ultimo arrancara el webappool. 


El template completo puede verse en [este repositorio](https://gitlab.ar.bsch/santander-tecnologia/global-ci-templates/-/blob/latest/common/.deploy-IIS.yml).

### GitFlow
La estrategia de branches propuesta para todos los equipos es Gitflow. En la misma, existen las siguientes branches:
- Master.
- Staging.
- Development.
- Features.
- Hotfixes.

El movimiento entre branches es exclusivamente a través de Merge Requests, y requiere que los mismos se aprueben por miembros del equipo, acorde a la branch de destino.

##### Master
Es la única branch no volátil del repositorio. Contiene el código que se deploya en producción, y debe tener un tag acorde al mismo. Toda branch partirá siempre a partir de ésta.

##### Features
Aquí se programa la solución. Como se comentó antes, su origen es `master`, y su nombre debe comenzar con la palabra `feature`. Una vez el código esté listo, __el desarrollador__ debe crear un Merge Request a `development`. El merge request deberá ser aprobado por __otro miembro del equipo que no haya participado de este desarrollo__. __El Team Leader__ es el encargado de Mergear el código, una vez la pipeline asociada al merge request sea exitosa y el mismo esté aprobado. Una vez mergeado el código, la branch debe ser eliminada.

##### Development
Aquí se solucionan los conflictos entre las distintas branches de feature. Como se comentó antes, su origen es `master`, y su nombre debe comenzar con la palabra `dev`. Recomendamos `development` para su nombre, aunque acepta alternativas pero se deberan tener en cuenta para los nombres del deploy-<envrionment>.json y el directorio de la configuracion. Debe coincidir sus nombres. Una vez el código esté listo, __el desarrollador__ debe crear un Merge Request a `staging`. El merge request deberá ser aprobado por __el Team Leader__. __Un representante de Quality Assurance__ (en caso de contar con dicho rol) es el encargado de Mergear el código, una vez la pipeline asociada al merge request sea exitosa y el mismo esté aprobado. Una vez mergeado el código, la branch debe ser eliminada.

##### Staging
Aquí se prepara el entregable, mezclando el código de `development` con `master`, y resolviendo posibles conflictos por eventuales hotfixes. Como se comentó antes, su origen es `master`, y su nombre debe ser `staging`. Una vez el código esté listo, __el QA Leader__ debe crear un Merge Request a `staging`. El merge request deberá ser aprobado por __el Head o el Sponsor__. __El QA Leader__ es el encargado de Mergear el código, una vez la pipeline asociada al merge request sea exitosa y el mismo esté aprobado. Una vez mergeado el código, la branch debe ser eliminada. Staging es el step en el cual se deben ejecutar las User Acceptance Tests.

##### Hotfixes
Aquí se programan arreglos ad-hoc de vicios ocultos que hubieran llegado inadvertidamente a master. Como se comentó antes, su origen es `master`, y su nombre debe comenzar con la palabra hotfix. Una vez el código esté listo, se deberán seguir los mismos pasos que si fuera una branch de `Features`, si bien se entiende que se podrán crear branches paralelas de `Development` y `Staging`, para llegar con la mayor agilidad posible a Master.


### Ejecución de la Pipeline
Existen 2 pipelines distintas:
- Merge Request Pipeline.
- Branch Pipeline.

##### Merge Request y branch Pipeline
Ejecuta los test y recupera del código, para analizarlo con Sonar.

##### Branch Pipeline
Deploya en los entornos comentados previamente y los branches que matcheen con "dev".

### Dudas
Para mayor información, siempre se puede consultar la [Guía básica de Gitlab Santander Tecnología](https://confluence.ar.bsch/display/DEV/DevOps). En caso de no encontrar la respuesta en la misma, comunicarse por Slack, a través del canal __##gitlab-legacy__.