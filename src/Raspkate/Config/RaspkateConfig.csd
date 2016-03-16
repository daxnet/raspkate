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
        <attributeProperty name="Prefix" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="prefix" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Modules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="modules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/897cec43-7d1f-49a8-918f-751995129163/ModuleElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="ModuleElement">
      <attributeProperties>
        <attributeProperty name="Path" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="path" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="IsRelative" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="relative" isReadOnly="false" defaultValue="true">
          <type>
            <externalTypeMoniker name="/897cec43-7d1f-49a8-918f-751995129163/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="ModuleElementCollection" collectionType="AddRemoveClearMapAlternate" xmlItemName="module" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/897cec43-7d1f-49a8-918f-751995129163/ModuleElement" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>