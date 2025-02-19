// Copyright 2013-2015 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Serilog.Capturing;

class MessageTemplateProcessor : ILogEventPropertyFactory, ILogEventPropertyValueFactory
{
    readonly MessageTemplateCache _parser = new(new MessageTemplateParser());
    readonly PropertyBinder _propertyBinder;
    readonly PropertyValueConverter _propertyValueConverter;

    public MessageTemplateProcessor(PropertyValueConverter propertyValueConverter)
    {
        _propertyValueConverter = propertyValueConverter;
        _propertyBinder = new(_propertyValueConverter);
    }

    public void Process(string messageTemplate, object?[]? messageTemplateParameters, out MessageTemplate parsedTemplate, out EventProperty[] properties)
    {
        parsedTemplate = _parser.Parse(messageTemplate);
        properties = _propertyBinder.ConstructProperties(parsedTemplate, messageTemplateParameters);
    }

    public LogEventProperty CreateProperty(string name, object? value, bool destructureObjects = false)
    {
        return _propertyValueConverter.CreateProperty(name, value, destructureObjects);
    }

    public LogEventPropertyValue CreatePropertyValue(object? value, bool destructureObjects = false)
    {
        return _propertyValueConverter.CreatePropertyValue(value, destructureObjects);
    }
}
