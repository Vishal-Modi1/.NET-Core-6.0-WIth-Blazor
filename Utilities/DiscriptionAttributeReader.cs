using System.ComponentModel;
using System.Reflection;

namespace GlobalUtilities
{
    public class DiscriptionAttributeReader
    {
        public static string GetDisplayText(MemberInfo[] memberInfo)
        {
            string value = "";
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
                else
                {
                    return memberInfo[0].Name.ToString();
                }
            }

            //If we have no description attribute, just return the ToString of the enum
            return value.ToString();
        }
    }

}
