using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CustomVariableStorage : VariableStorageBehaviour
{

    /// Where we actually keeping our variables
    private Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

    [System.Serializable]
    public class DefaultVariable
    {
        /// <summary>
        /// The name of the variable.
        /// </summary>
        /// <remarks>
        /// Do not include the `$` prefix in front of the variable
        /// name. It will be added for you.
        /// </remarks>
        public string name;

        /// <summary>
        /// The value of the variable, as a string.
        /// </summary>
        /// <remarks>
        /// This string will be converted to the appropriate type,
        /// depending on the value of <see cref="type"/>.
        /// </remarks>
        public string value;

        /// <summary>
        /// The type of the variable.
        /// </summary>
        public Yarn.Value.Type type;
    }

    /// <summary>
    /// The list of default variables that should be present in the
    /// InMemoryVariableStorage when the scene loads.
    /// </summary>
    public DefaultVariable[] defaultVariables;

    // Store a value into a variable
    public override void SetValue(string variableName, Yarn.Value value)
    {
        // 'variableName' is the name of the variable that 'value' 
        // should be stored in.
    }

    // Return a value, given a variable name
    public override Yarn.Value GetValue(string variableName) // RETURN Yarn.Value PLEASEEEE
    {
        // 'variableName' is the name of the variable to return a value for

            return Yarn.Value.NULL;

    }

    // Return to the original state
    public override void ResetToDefaults()
    {

    }
}
