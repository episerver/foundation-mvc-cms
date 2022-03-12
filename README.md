<a href="https://github.com/episerver/foundation-mvc-cms"><img src="https://www.optimizely.com/globalassets/02.-global-images/navigation/optimizely_logo_navigation.svg" title="Foundation" alt="Foundation"></a>

## Foundation 

Foundation offers a starting point that is intuitive, well-structured and modular allowing developers to explore CMS, Search amd Navigaion, Data Platform and Experimentation.

For an instance of Foundation that includes commerce: <a href="https://github.com/episerver/Foundation">https://github.com/episerver/foundation-mvc-cms</a>

---

## Prerequisites

You will need these to run locally on your machine.

[Net 5](https://dotnet.microsoft.com/download/dotnet/5.0) sdk is required to use with visual studio.  Runtime maybe sufficent to just run the application.

[Node JS](https://nodejs.org/en/download/)

Mac/Linux

[Docker](https://docs.docker.com/desktop/mac/install/)

Windows

[Sql Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

## The Solution

`Foundation has a default username and password of admin@example.com / Episerver123!`

---

## Installation

### Windows

```
open command prompt as administrator
git clone https://github.com/episerver/foundation-mvc-cms.git
cd foundation-mvc-cms
git checkout net5
setup.cmd 
dotnet run --project .\src\Foundation\Foundation.csproj
```

### Mac

```
Open a Terminal window
git clone https://github.com/episerver/foundation-mvc-cms.git
cd foundation-mvc-cms
git checkout net5
chmod u+x setup.sh
./setup.sh
dotnet run --project ./src/Foundation/Foundation.csproj
```

### Linux

```
Open a bash terminal window
git clone https://github.com/episerver/foundation-mvc-cms.git
cd foundation-mvc-cms
git checkout net5
chmod u+x setup.sh
./setup.sh
dotnet run --project ./src/Foundation/Foundation.csproj
```

### View the site

After completing the setup steps and running the solution, access the site at <a href="http://localhost:5000">http://localhost:5000</a>.

To change the default port, modify the file <a href="https://github.com/episerver/foundation-mvc-cms/blob/net5/src/Foundation/Properties/launchSettings.json">/src/Foundation/Properties/launchSettings.json</a>.

---