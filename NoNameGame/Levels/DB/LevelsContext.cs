using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;

#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoNameGame.Levels.DB
{
    public class DebugWriter : TextWriter
{
    private const int DefaultBufferSize = 256;
    private System.Text.StringBuilder _buffer;

    public DebugWriter()
    {
        BufferSize = 256;
        _buffer = new System.Text.StringBuilder(BufferSize);
    }

    public int BufferSize
    {
        get;
        private set;
    }

    public override System.Text.Encoding Encoding
    {
        get { return System.Text.Encoding.UTF8; }
    }

    #region StreamWriter Overrides
    public override void Write(char value)
    {
        _buffer.Append(value);
        if (_buffer.Length >= BufferSize)
            Flush();
    }

    public override void WriteLine(string value)
    {
        Flush();

        using(var reader = new StringReader(value))
        {
            string line; 
            while( null != (line = reader.ReadLine()))
                System.Diagnostics.Debug.WriteLine(line);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            Flush();
    }

    public override void Flush()
    {
        if (_buffer.Length > 0)
        {
            System.Diagnostics.Debug.WriteLine(_buffer);
            _buffer.Clear();
        }
    }
    #endregion
}


	public partial class LevelsContext : System.Data.Linq.DataContext
	{
		
		public bool CreateIfNotExists()
		{
			bool created = false;
			if (!this.DatabaseExists())
			{
				string[] names = this.GetType().Assembly.GetManifestResourceNames();
				string name = names.Where(n => n.EndsWith(FileName)).FirstOrDefault();
				if (name != null)
				{
					using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
					{
						if (resourceStream != null)
						{
							using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
							{
								using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(FileName, FileMode.Create, myIsolatedStorage))
								{
									using (BinaryWriter writer = new BinaryWriter(fileStream))
									{
										long length = resourceStream.Length;
										byte[] buffer = new byte[32];
										int readCount = 0;
										using (BinaryReader reader = new BinaryReader(resourceStream))
										{
											// read file in chunks in order to reduce memory consumption and increase performance
											while (readCount < length)
											{
												int actual = reader.Read(buffer, 0, buffer.Length);
												readCount += actual;
												writer.Write(buffer, 0, actual);
											}
										}
									}
								}
							}
							created = true;
						}
						else
						{
							this.CreateDatabase();
							created = true;
						}
					}
				}
				else
				{
					this.CreateDatabase();
					created = true;
				}
			}
			return created;
		}
		
		public bool LogDebug
		{
			set
			{
				if (value)
				{
					this.Log = new DebugWriter();
				}
			}
		}
		
		public static string ConnectionString = "Data Source=isostore:/Levels.sdf";

		public static string ConnectionStringReadOnly = "Data Source=appdata:/Levels.sdf;File Mode=Read Only;";

		public static string FileName = "Levels.sdf";

		public LevelsContext(string connectionString) : base(connectionString)
		{
			OnCreated();
		}
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertDisabledAreas(DisabledAreas instance);
    partial void UpdateDisabledAreas(DisabledAreas instance);
    partial void DeleteDisabledAreas(DisabledAreas instance);
    partial void InsertLevelData(LevelData instance);
    partial void UpdateLevelData(LevelData instance);
    partial void DeleteLevelData(LevelData instance);
    partial void InsertLevelGroup(LevelGroup instance);
    partial void UpdateLevelGroup(LevelGroup instance);
    partial void DeleteLevelGroup(LevelGroup instance);
    partial void InsertLevelPack(LevelPack instance);
    partial void UpdateLevelPack(LevelPack instance);
    partial void DeleteLevelPack(LevelPack instance);
    #endregion
		
		public System.Data.Linq.Table<DisabledAreas> DisabledAreas
		{
			get
			{
				return this.GetTable<DisabledAreas>();
			}
		}
		
		public System.Data.Linq.Table<LevelData> LevelData
		{
			get
			{
				return this.GetTable<LevelData>();
			}
		}
		
		public System.Data.Linq.Table<LevelGroup> LevelGroup
		{
			get
			{
				return this.GetTable<LevelGroup>();
			}
		}
		
		public System.Data.Linq.Table<LevelPack> LevelPack
		{
			get
			{
				return this.GetTable<LevelPack>();
			}
		}
	}
	
	[Table()]
	public partial class DisabledAreas : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Coordinates;
		
		private int _BoardSize;
		
		private EntitySet<LevelData> _LevelData;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCoordinatesChanging(string value);
    partial void OnCoordinatesChanged();
    partial void OnBoardSizeChanging(int value);
    partial void OnBoardSizeChanged();
    #endregion
		
		public DisabledAreas()
		{
			this._LevelData = new EntitySet<LevelData>(new Action<LevelData>(this.attach_LevelData), new Action<LevelData>(this.detach_LevelData));
			OnCreated();
		}
		
		[Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[Column(Storage="_Coordinates", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string Coordinates
		{
			get
			{
				return this._Coordinates;
			}
			set
			{
				if ((this._Coordinates != value))
				{
					this.OnCoordinatesChanging(value);
					this.SendPropertyChanging();
					this._Coordinates = value;
					this.SendPropertyChanged("Coordinates");
					this.OnCoordinatesChanged();
				}
			}
		}
		
		[Column(Storage="_BoardSize", DbType="Int NOT NULL")]
		public int BoardSize
		{
			get
			{
				return this._BoardSize;
			}
			set
			{
				if ((this._BoardSize != value))
				{
					this.OnBoardSizeChanging(value);
					this.SendPropertyChanging();
					this._BoardSize = value;
					this.SendPropertyChanged("BoardSize");
					this.OnBoardSizeChanged();
				}
			}
		}
		
		[global::System.Runtime.Serialization.IgnoreDataMember]
		[Association(Name="LevelData_DisabledAreas", Storage="_LevelData", ThisKey="Id", OtherKey="DisabledAreasId", DeleteRule="NO ACTION")]
		public EntitySet<LevelData> LevelData
		{
			get
			{
				return this._LevelData;
			}
			set
			{
				this._LevelData.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_LevelData(LevelData entity)
		{
			this.SendPropertyChanging();
			entity.DisabledAreas = this;
		}
		
		private void detach_LevelData(LevelData entity)
		{
			this.SendPropertyChanging();
			entity.DisabledAreas = null;
		}
	}
	
	[Table()]
	public partial class LevelData : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _MovesCount;
		
		private int _Difficulty;
		
		private string _Moves;
		
		private int _LevelGroupId;
		
		private string _DisplayName;
		
		private int _OrderNo;
		
		private byte _BoardSize;
		
		private System.Nullable<byte> _TutorialStep;
		
		private System.Nullable<int> _DisabledAreasId;
		
		private EntityRef<DisabledAreas> _DisabledAreas;
		
		private EntityRef<LevelGroup> _LevelGroup;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnMovesCountChanging(int value);
    partial void OnMovesCountChanged();
    partial void OnDifficultyChanging(int value);
    partial void OnDifficultyChanged();
    partial void OnMovesChanging(string value);
    partial void OnMovesChanged();
    partial void OnLevelGroupIdChanging(int value);
    partial void OnLevelGroupIdChanged();
    partial void OnDisplayNameChanging(string value);
    partial void OnDisplayNameChanged();
    partial void OnOrderNoChanging(int value);
    partial void OnOrderNoChanged();
    partial void OnBoardSizeChanging(byte value);
    partial void OnBoardSizeChanged();
    partial void OnTutorialStepChanging(System.Nullable<byte> value);
    partial void OnTutorialStepChanged();
    partial void OnDisabledAreasIdChanging(System.Nullable<int> value);
    partial void OnDisabledAreasIdChanged();
    #endregion
		
		public LevelData()
		{
			this._DisabledAreas = default(EntityRef<DisabledAreas>);
			this._LevelGroup = default(EntityRef<LevelGroup>);
			OnCreated();
		}
		
		[Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[Column(Storage="_MovesCount", DbType="Int NOT NULL")]
		public int MovesCount
		{
			get
			{
				return this._MovesCount;
			}
			set
			{
				if ((this._MovesCount != value))
				{
					this.OnMovesCountChanging(value);
					this.SendPropertyChanging();
					this._MovesCount = value;
					this.SendPropertyChanged("MovesCount");
					this.OnMovesCountChanged();
				}
			}
		}
		
		[Column(Storage="_Difficulty", DbType="Int NOT NULL")]
		public int Difficulty
		{
			get
			{
				return this._Difficulty;
			}
			set
			{
				if ((this._Difficulty != value))
				{
					this.OnDifficultyChanging(value);
					this.SendPropertyChanging();
					this._Difficulty = value;
					this.SendPropertyChanged("Difficulty");
					this.OnDifficultyChanged();
				}
			}
		}
		
		[Column(Storage="_Moves", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Moves
		{
			get
			{
				return this._Moves;
			}
			set
			{
				if ((this._Moves != value))
				{
					this.OnMovesChanging(value);
					this.SendPropertyChanging();
					this._Moves = value;
					this.SendPropertyChanged("Moves");
					this.OnMovesChanged();
				}
			}
		}
		
		[Column(Storage="_LevelGroupId", DbType="Int NOT NULL")]
		public int LevelGroupId
		{
			get
			{
				return this._LevelGroupId;
			}
			set
			{
				if ((this._LevelGroupId != value))
				{
					if (this._LevelGroup.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnLevelGroupIdChanging(value);
					this.SendPropertyChanging();
					this._LevelGroupId = value;
					this.SendPropertyChanged("LevelGroupId");
					this.OnLevelGroupIdChanged();
				}
			}
		}
		
		[Column(Storage="_DisplayName", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string DisplayName
		{
			get
			{
				return this._DisplayName;
			}
			set
			{
				if ((this._DisplayName != value))
				{
					this.OnDisplayNameChanging(value);
					this.SendPropertyChanging();
					this._DisplayName = value;
					this.SendPropertyChanged("DisplayName");
					this.OnDisplayNameChanged();
				}
			}
		}
		
		[Column(Storage="_OrderNo", DbType="Int NOT NULL")]
		public int OrderNo
		{
			get
			{
				return this._OrderNo;
			}
			set
			{
				if ((this._OrderNo != value))
				{
					this.OnOrderNoChanging(value);
					this.SendPropertyChanging();
					this._OrderNo = value;
					this.SendPropertyChanged("OrderNo");
					this.OnOrderNoChanged();
				}
			}
		}
		
		[Column(Storage="_BoardSize", DbType="TinyInt NOT NULL")]
		public byte BoardSize
		{
			get
			{
				return this._BoardSize;
			}
			set
			{
				if ((this._BoardSize != value))
				{
					this.OnBoardSizeChanging(value);
					this.SendPropertyChanging();
					this._BoardSize = value;
					this.SendPropertyChanged("BoardSize");
					this.OnBoardSizeChanged();
				}
			}
		}
		
		[Column(Storage="_TutorialStep", DbType="TinyInt")]
		public System.Nullable<byte> TutorialStep
		{
			get
			{
				return this._TutorialStep;
			}
			set
			{
				if ((this._TutorialStep != value))
				{
					this.OnTutorialStepChanging(value);
					this.SendPropertyChanging();
					this._TutorialStep = value;
					this.SendPropertyChanged("TutorialStep");
					this.OnTutorialStepChanged();
				}
			}
		}
		
		[Column(Storage="_DisabledAreasId", DbType="Int")]
		public System.Nullable<int> DisabledAreasId
		{
			get
			{
				return this._DisabledAreasId;
			}
			set
			{
				if ((this._DisabledAreasId != value))
				{
					if (this._DisabledAreas.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnDisabledAreasIdChanging(value);
					this.SendPropertyChanging();
					this._DisabledAreasId = value;
					this.SendPropertyChanged("DisabledAreasId");
					this.OnDisabledAreasIdChanged();
				}
			}
		}
		
		[global::System.Runtime.Serialization.IgnoreDataMember]
		[Association(Name="LevelData_DisabledAreas", Storage="_DisabledAreas", ThisKey="DisabledAreasId", OtherKey="Id", IsForeignKey=true)]
		public DisabledAreas DisabledAreas
		{
			get
			{
				return this._DisabledAreas.Entity;
			}
			set
			{
				DisabledAreas previousValue = this._DisabledAreas.Entity;
				if (((previousValue != value) 
							|| (this._DisabledAreas.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._DisabledAreas.Entity = null;
						previousValue.LevelData.Remove(this);
					}
					this._DisabledAreas.Entity = value;
					if ((value != null))
					{
						value.LevelData.Add(this);
						this._DisabledAreasId = value.Id;
					}
					else
					{
						this._DisabledAreasId = default(Nullable<int>);
					}
					this.SendPropertyChanged("DisabledAreas");
				}
			}
		}
		
		[global::System.Runtime.Serialization.IgnoreDataMember]
		[Association(Name="LevelData_LevelGroup", Storage="_LevelGroup", ThisKey="LevelGroupId", OtherKey="Id", IsForeignKey=true)]
		public LevelGroup LevelGroup
		{
			get
			{
				return this._LevelGroup.Entity;
			}
			set
			{
				LevelGroup previousValue = this._LevelGroup.Entity;
				if (((previousValue != value) 
							|| (this._LevelGroup.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._LevelGroup.Entity = null;
						previousValue.LevelData.Remove(this);
					}
					this._LevelGroup.Entity = value;
					if ((value != null))
					{
						value.LevelData.Add(this);
						this._LevelGroupId = value.Id;
					}
					else
					{
						this._LevelGroupId = default(int);
					}
					this.SendPropertyChanged("LevelGroup");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table()]
	public partial class LevelGroup : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private string _Description;
		
		private int _LevelPackId;
		
		private byte _OrderNo;
		
		private bool _AllLevelsUnlocked;
		
		private string _Name_PL;
		
		private string _Name_ES;
		
		private EntitySet<LevelData> _LevelData;
		
		private EntityRef<LevelPack> _LevelPack;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnLevelPackIdChanging(int value);
    partial void OnLevelPackIdChanged();
    partial void OnOrderNoChanging(byte value);
    partial void OnOrderNoChanged();
    partial void OnAllLevelsUnlockedChanging(bool value);
    partial void OnAllLevelsUnlockedChanged();
    partial void OnName_PLChanging(string value);
    partial void OnName_PLChanged();
    partial void OnName_ESChanging(string value);
    partial void OnName_ESChanged();
    #endregion
		
		public LevelGroup()
		{
			this._LevelData = new EntitySet<LevelData>(new Action<LevelData>(this.attach_LevelData), new Action<LevelData>(this.detach_LevelData));
			this._LevelPack = default(EntityRef<LevelPack>);
			OnCreated();
		}
		
		[Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_Description", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[Column(Storage="_LevelPackId", DbType="Int NOT NULL")]
		public int LevelPackId
		{
			get
			{
				return this._LevelPackId;
			}
			set
			{
				if ((this._LevelPackId != value))
				{
					if (this._LevelPack.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnLevelPackIdChanging(value);
					this.SendPropertyChanging();
					this._LevelPackId = value;
					this.SendPropertyChanged("LevelPackId");
					this.OnLevelPackIdChanged();
				}
			}
		}
		
		[Column(Storage="_OrderNo", DbType="TinyInt NOT NULL")]
		public byte OrderNo
		{
			get
			{
				return this._OrderNo;
			}
			set
			{
				if ((this._OrderNo != value))
				{
					this.OnOrderNoChanging(value);
					this.SendPropertyChanging();
					this._OrderNo = value;
					this.SendPropertyChanged("OrderNo");
					this.OnOrderNoChanged();
				}
			}
		}
		
		[Column(Storage="_AllLevelsUnlocked", DbType="Bit NOT NULL")]
		public bool AllLevelsUnlocked
		{
			get
			{
				return this._AllLevelsUnlocked;
			}
			set
			{
				if ((this._AllLevelsUnlocked != value))
				{
					this.OnAllLevelsUnlockedChanging(value);
					this.SendPropertyChanging();
					this._AllLevelsUnlocked = value;
					this.SendPropertyChanged("AllLevelsUnlocked");
					this.OnAllLevelsUnlockedChanged();
				}
			}
		}
		
		[Column(Storage="_Name_PL", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name_PL
		{
			get
			{
				return this._Name_PL;
			}
			set
			{
				if ((this._Name_PL != value))
				{
					this.OnName_PLChanging(value);
					this.SendPropertyChanging();
					this._Name_PL = value;
					this.SendPropertyChanged("Name_PL");
					this.OnName_PLChanged();
				}
			}
		}
		
		[Column(Storage="_Name_ES", DbType="NVarChar(25)")]
		public string Name_ES
		{
			get
			{
				return this._Name_ES;
			}
			set
			{
				if ((this._Name_ES != value))
				{
					this.OnName_ESChanging(value);
					this.SendPropertyChanging();
					this._Name_ES = value;
					this.SendPropertyChanged("Name_ES");
					this.OnName_ESChanged();
				}
			}
		}
		
		[global::System.Runtime.Serialization.IgnoreDataMember]
		[Association(Name="LevelData_LevelGroup", Storage="_LevelData", ThisKey="Id", OtherKey="LevelGroupId", DeleteRule="NO ACTION")]
		public EntitySet<LevelData> LevelData
		{
			get
			{
				return this._LevelData;
			}
			set
			{
				this._LevelData.Assign(value);
			}
		}
		
		[global::System.Runtime.Serialization.IgnoreDataMember]
		[Association(Name="LevelGroup_LevelPack", Storage="_LevelPack", ThisKey="LevelPackId", OtherKey="Id", IsForeignKey=true)]
		public LevelPack LevelPack
		{
			get
			{
				return this._LevelPack.Entity;
			}
			set
			{
				LevelPack previousValue = this._LevelPack.Entity;
				if (((previousValue != value) 
							|| (this._LevelPack.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._LevelPack.Entity = null;
						previousValue.LevelGroup.Remove(this);
					}
					this._LevelPack.Entity = value;
					if ((value != null))
					{
						value.LevelGroup.Add(this);
						this._LevelPackId = value.Id;
					}
					else
					{
						this._LevelPackId = default(int);
					}
					this.SendPropertyChanged("LevelPack");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_LevelData(LevelData entity)
		{
			this.SendPropertyChanging();
			entity.LevelGroup = this;
		}
		
		private void detach_LevelData(LevelData entity)
		{
			this.SendPropertyChanging();
			entity.LevelGroup = null;
		}
	}
	
	[Table()]
	public partial class LevelPack : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private string _Description;
		
		private byte _OrderNo;
		
		private string _Name_PL;
		
		private string _Description_PL;
		
		private string _Description_ES;
		
		private EntitySet<LevelGroup> _LevelGroup;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnOrderNoChanging(byte value);
    partial void OnOrderNoChanged();
    partial void OnName_PLChanging(string value);
    partial void OnName_PLChanged();
    partial void OnDescription_PLChanging(string value);
    partial void OnDescription_PLChanged();
    partial void OnDescription_ESChanging(string value);
    partial void OnDescription_ESChanged();
    #endregion
		
		public LevelPack()
		{
			this._LevelGroup = new EntitySet<LevelGroup>(new Action<LevelGroup>(this.attach_LevelGroup), new Action<LevelGroup>(this.detach_LevelGroup));
			OnCreated();
		}
		
		[Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_Description", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[Column(Storage="_OrderNo", DbType="TinyInt NOT NULL")]
		public byte OrderNo
		{
			get
			{
				return this._OrderNo;
			}
			set
			{
				if ((this._OrderNo != value))
				{
					this.OnOrderNoChanging(value);
					this.SendPropertyChanging();
					this._OrderNo = value;
					this.SendPropertyChanged("OrderNo");
					this.OnOrderNoChanged();
				}
			}
		}
		
		[Column(Storage="_Name_PL", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name_PL
		{
			get
			{
				return this._Name_PL;
			}
			set
			{
				if ((this._Name_PL != value))
				{
					this.OnName_PLChanging(value);
					this.SendPropertyChanging();
					this._Name_PL = value;
					this.SendPropertyChanged("Name_PL");
					this.OnName_PLChanged();
				}
			}
		}
		
		[Column(Storage="_Description_PL", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Description_PL
		{
			get
			{
				return this._Description_PL;
			}
			set
			{
				if ((this._Description_PL != value))
				{
					this.OnDescription_PLChanging(value);
					this.SendPropertyChanging();
					this._Description_PL = value;
					this.SendPropertyChanged("Description_PL");
					this.OnDescription_PLChanged();
				}
			}
		}
		
		[Column(Storage="_Description_ES", DbType="NVarChar(100)")]
		public string Description_ES
		{
			get
			{
				return this._Description_ES;
			}
			set
			{
				if ((this._Description_ES != value))
				{
					this.OnDescription_ESChanging(value);
					this.SendPropertyChanging();
					this._Description_ES = value;
					this.SendPropertyChanged("Description_ES");
					this.OnDescription_ESChanged();
				}
			}
		}
		
		[global::System.Runtime.Serialization.IgnoreDataMember]
		[Association(Name="LevelGroup_LevelPack", Storage="_LevelGroup", ThisKey="Id", OtherKey="LevelPackId", DeleteRule="NO ACTION")]
		public EntitySet<LevelGroup> LevelGroup
		{
			get
			{
				return this._LevelGroup;
			}
			set
			{
				this._LevelGroup.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_LevelGroup(LevelGroup entity)
		{
			this.SendPropertyChanging();
			entity.LevelPack = this;
		}
		
		private void detach_LevelGroup(LevelGroup entity)
		{
			this.SendPropertyChanging();
			entity.LevelPack = null;
		}
	}
}
#pragma warning restore 1591
