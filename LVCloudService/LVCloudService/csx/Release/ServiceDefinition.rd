<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="LVCloudService" generation="1" functional="0" release="0" Id="67d9c5f2-b69b-4ce0-ba24-b70c4aa108dd" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="LVCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="CloudDataService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/LVCloudService/LVCloudServiceGroup/LB:CloudDataService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="CloudDataService:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/LVCloudService/LVCloudServiceGroup/MapCloudDataService:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="CloudDataService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/LVCloudService/LVCloudServiceGroup/MapCloudDataService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="CloudDataServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/LVCloudService/LVCloudServiceGroup/MapCloudDataServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:CloudDataService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCloudDataService:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataService/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapCloudDataService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapCloudDataServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CloudDataService" generation="1" functional="0" release="0" software="C:\Users\email\Source\Repos\MyCONVENO\Sales4ProCloud-LivingKitzbuehel\LVCloudService\LVCloudService\csx\Release\roles\CloudDataService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CloudDataService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CloudDataService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="CloudDataServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="CloudDataServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="CloudDataServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="fd2aea9d-ac8a-4f84-ad8b-87a88bb8ef42" ref="Microsoft.RedDog.Contract\ServiceContract\LVCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="07c63483-bdf6-4dbc-9e22-20be8a42b3ff" ref="Microsoft.RedDog.Contract\Interface\CloudDataService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/LVCloudService/LVCloudServiceGroup/CloudDataService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>