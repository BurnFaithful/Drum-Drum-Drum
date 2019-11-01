using UnityEngine;

public class AliasAttribute : PropertyAttribute
{
    public string Alias { get; private set; }

    public AliasAttribute(string alias = "Not a valid alias!!") => this.Alias = alias;
}