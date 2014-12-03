<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CFPress.UmbracoMVCAzureCloudServiec" generation="1" functional="0" release="0" Id="0ac45fdc-ffc9-4f49-a671-d152643f2b97" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CFPress.UmbracoMVCAzureCloudServiecGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="CFPress.UmbracoMVCApplication:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/LB:CFPress.UmbracoMVCApplication:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="CFPress.UmbracoMVCApplication:UmbracoConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/MapCFPress.UmbracoMVCApplication:UmbracoConnectionString" />
          </maps>
        </aCS>
        <aCS name="CFPress.UmbracoMVCApplicationInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/MapCFPress.UmbracoMVCApplicationInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:CFPress.UmbracoMVCApplication:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplication/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCFPress.UmbracoMVCApplication:UmbracoConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplication/UmbracoConnectionString" />
          </setting>
        </map>
        <map name="MapCFPress.UmbracoMVCApplicationInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplicationInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CFPress.UmbracoMVCApplication" generation="1" functional="0" release="0" software="C:\Users\snageswaran\Documents\CFPress.UmbracoMVCAzureSolution\CFPress.UmbracoMVCAzureCloudServiec\csx\Release\roles\CFPress.UmbracoMVCApplication" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="UmbracoConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CFPress.UmbracoMVCApplication&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CFPress.UmbracoMVCApplication&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplicationInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplicationUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplicationFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="CFPress.UmbracoMVCApplicationUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="CFPress.UmbracoMVCApplicationFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="CFPress.UmbracoMVCApplicationInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="5bd532d3-c2a1-47c8-8596-6274beeb082b" ref="Microsoft.RedDog.Contract\ServiceContract\CFPress.UmbracoMVCAzureCloudServiecContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="22117f6a-2dc0-41e5-a684-18fcc5845f64" ref="Microsoft.RedDog.Contract\Interface\CFPress.UmbracoMVCApplication:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplication:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>