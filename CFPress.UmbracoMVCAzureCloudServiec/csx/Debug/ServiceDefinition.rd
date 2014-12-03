<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CFPress.UmbracoMVCAzureCloudServiec" generation="1" functional="0" release="0" Id="ac5f544a-4a62-4598-821b-5410d542906f" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
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
          <role name="CFPress.UmbracoMVCApplication" generation="1" functional="0" release="0" software="C:\Users\snageswaran\Documents\CFPress.UmbracoMVCAzureSolution\CFPress.UmbracoMVCAzureCloudServiec\csx\Debug\roles\CFPress.UmbracoMVCApplication" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
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
    <implementation Id="d4837f99-b5a0-4aa1-8c54-70fc5973ea28" ref="Microsoft.RedDog.Contract\ServiceContract\CFPress.UmbracoMVCAzureCloudServiecContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="fffcfda8-b784-447d-87f3-00229be61dfd" ref="Microsoft.RedDog.Contract\Interface\CFPress.UmbracoMVCApplication:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CFPress.UmbracoMVCAzureCloudServiec/CFPress.UmbracoMVCAzureCloudServiecGroup/CFPress.UmbracoMVCApplication:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>