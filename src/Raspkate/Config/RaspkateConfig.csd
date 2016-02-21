<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="897cec43-7d1f-49a8-918f-751995129163" namespace="Raspkate.Config" xmlSchemaNamespace="urn:Raspkate.Config" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="RaspkateConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="raspkateConfiguration">
      <attributeProperties>
        <attributeProperty name="BasePath" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="basePath" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Prefix" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="prefix" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Relative" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="relative" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Handlers" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="handlers" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/897cec43-7d1f-49a8-918f-751995129163/HandlerElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="HandlerElementCollection" xmlItemName="handler" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/897cec43-7d1f-49a8-918f-751995129163/HandlerElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="HandlerElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="HandlerProperties" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="properties" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/897cec43-7d1f-49a8-918f-751995129163/HandlerPropertyElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="HandlerPropertyElementCollection" xmlItemName="property" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/897cec43-7d1f-49a8-918f-751995129163/HandlerPropertyElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="HandlerPropertyElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>