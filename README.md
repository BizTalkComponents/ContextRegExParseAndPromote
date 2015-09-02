[![Build status](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/contextregexparseandpromote?branch=master)](https://ci.appveyor.com/api/projects/status/github/BizTalkComponents/contextregexparseandpromote/branch/master)

##Description
PipelineComponent that uses a regular expression to parse a context property and promote the result to another context property.


| Parameter          | Description                                                                                 | Type | Validation                        |
| -------------------|---------------------------------------------------------------------------------------------|------|-----------------------------------|
|Property to parse|The property path of the property to parse.|String|Required, Format=namespace#property|
|RegEx Pattern|The regular expression pattern to use to parse the property value.|String|Required|
|Destination property|The property path to promote the result to.|String|Required, Format=namespace#property|
|Throw if no match|Specifies whether an ArgumentException should be thrown if the pattern does not match any value.|Bool|Required|