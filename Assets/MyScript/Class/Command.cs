//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class Command// : ISerializable 
{
	private CommandCode command_Code;
	private object data1;
	private object data2;
	private object data3;
	private object data4;
	private object data5;
	
	
	#region Property
	public CommandCode Command_Code
	{
		get { return command_Code; }
		set { command_Code = value; }
	}
	
	private byte[] _data1;
	
	public object Data1
	{
		get { return data1; }
		set { data1 = value; }
	}
	
	private byte[] _data2;
	
	public object Data2
	{
		get { return data2; }
		set { data2 = value; }
	}
	
	private byte[] _data3;
	
	public object Data3
	{
		get { return data3; }
		set { data3 = value; }
	}
	
	private byte[] _data4;
	
	public object Data4
	{
		get { return data4; }
		set { data4 = value; }
	}
	
	private byte[] _data5;
	
	public object Data5
	{
		get { return data5; }
		set { data5 = value; }
	}
	
	#endregion
	
	#region Constructors
	public Command()
	{ }
	
	public Command(CommandCode Command_Code)
	{
		this.Command_Code = Command_Code;
	}
	
	public Command(CommandCode Command_Code, object Data1)
	{
		this.Command_Code = Command_Code;
		this.Data1 = Data1;
	}
	public Command(CommandCode Command_Code, object Data1, object Data2)
	{
		this.Command_Code = Command_Code;
		this.Data1 = Data1;
		this.Data2 = Data2;
	}
	public Command(CommandCode Command_Code, object Data1, object Data2, object Data3)
	{
		this.Command_Code = Command_Code;
		this.Data1 = Data1;
		this.Data2 = Data2;
		this.Data3 = Data3;
	}
	public Command(CommandCode Command_Code, object Data1, object Data2, object Data3, object Data4)
	{
		this.Command_Code = Command_Code;
		this.Data1 = Data1;
		this.Data2 = Data2;
		this.Data3 = Data3;
		this.Data4 = Data4;
	}
	public Command(CommandCode Command_Code, object Data1, object Data2, object Data3, object Data4, object Data5)
	{
		this.Command_Code = Command_Code;
		this.Data1 = Data1;
		this.Data2 = Data2;
		this.Data3 = Data3;
		this.Data4 = Data4;
		this.Data5 = Data5;
	}
	
	public Command(byte[] bytes)
	{
		this.Deserialize(bytes);
		//var mystring = Encoding.Unicode.GetString(bytes);
		//Command temp = JsonConvert.DeserializeObject<Command>(mystring);
	}
	#endregion

	
	public byte[] Serialize()
	{
		#region Old Serialize
		MemoryStream stream = new MemoryStream();
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Serialize(stream, this);
		return stream.ToArray();
		#endregion		
	}
	
	public void Deserialize(byte[] bytes)
	{
		
		#region Old Deserialize
		BinaryFormatter bformatter = new BinaryFormatter();
		MemoryStream stream = new MemoryStream(bytes);
		Command temp = new Command(CommandCode.DeserializeFailed);
		try
		{
			temp = (Command)bformatter.Deserialize(stream);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
			temp = new Command(CommandCode.DeserializeFailed);
		}
		
		this.Command_Code = temp.Command_Code;
		if (temp.Data1 != null) this.Data1 = temp.Data1;
		if (temp.Data2 != null) this.Data2 = temp.Data2;
		if (temp.Data3 != null) this.Data3 = temp.Data3;
		if (temp.Data4 != null) this.Data4 = temp.Data4;
		if (temp.Data5 != null) this.Data5 = temp.Data5;
		#endregion
		
	}
	
}

