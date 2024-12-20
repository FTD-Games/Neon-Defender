using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Profile
{
    private const string PROFILE_NAME = "PlayerProfile.data";

    private string _path;
    public enum E_Destination { None, Local, Cloud }
    public E_Destination destination;

    /// <summary>
    /// Defines all primitives single properties of the profile.
    /// Here properties can be added and deleted without issues.
    /// ATTENTION: MIGRATION IS ONLY PROCESSED, IF THE APP VERSION IS CHANGED !!!!!!!!!!
    /// </summary>
    public List<PropSaver> allProps = new List<PropSaver>
    {
            new PropSaver(E_PType.E_String, E_PName.ProfileVersion),
    };

    /// <summary>
    /// The type of the property used in the profile.
    /// </summary>
    public enum E_PType { E_Bool, E_String, E_Int, E_Float }

    /// <summary>
    /// The name of the property used in the profile.
    /// </summary>
    public enum E_PName { ProfileVersion }

    public Profile(bool isLocal)
    {
        destination = isLocal ? E_Destination.Local : E_Destination.Cloud;
        _path = $"{Application.persistentDataPath}/{PROFILE_NAME}";
    }


    /// <summary>
    /// Creates a new profile with default values set by code.
    /// </summary>
    public SaveData CreateNewProfile()
    {
        var newProfile = new SaveData(allProps);
        // Use the area below to define default values of the new created properties or the new created profile
        newProfile.SetString(E_PName.ProfileVersion, GameControl.control.profileVersion);
        return newProfile;
    }

    /// <summary>
    /// Saves the loaded profile to the target destination.
    /// </summary>
    public void SaveProfile(SaveData toSave)
    {
        switch (destination)
        {
            case E_Destination.Local:
                var bf = new BinaryFormatter();
                FileStream file = File.Create(_path);
                bf.Serialize(file, toSave);
                file.Close();
                break;
            case E_Destination.Cloud:
                break;
            default: return;
        }
    }

    /// <summary>
    /// Loads the profile from the target destination.
    /// </summary>
    public SaveData LoadProfile()
    {
        var data = new SaveData(allProps);
        switch (destination)
        {
            case E_Destination.Local:

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(_path, FileMode.Open);
                data = (SaveData)bf.Deserialize(file);
                file.Close();
                break;
            case E_Destination.Cloud:
                break;
        }
        return data;
    }

    /// <summary>
    /// Checks if the profile is existing on the target path.
    /// </summary>
    public bool ProfileExists()
    {
        switch (destination)
        {
            case E_Destination.Local:
                return File.Exists(_path);
            case E_Destination.Cloud:
                return false;
        }
        return false;
    }

    /// <summary>
    /// Checks if there is a migration process needed for the profile.
    /// 1. Game- and Profileversion
    /// 2. Property Lists
    /// </summary>
    public bool NeedsMigration(SaveData toCheck)
    {
        var newVersion = CreateNewProfile();

        if (newVersion.IsSameVersion(toCheck))
            return false;

        if (newVersion.AreBoolPropertiesDifferent(toCheck))
            return true;

        if (newVersion.AreIntPropertiesDifferent(toCheck))
            return true;

        if (newVersion.AreFloatPropertiesDifferent(toCheck))
            return true;

        if (newVersion.AreStringPropertiesDifferent(toCheck))
            return true;

        return true;
    }

    /// <summary>
    /// Makes the old profile usable in the new profile, without losing data.
    /// Allows to add or remove properties from profiles without reset progress.
    /// (!) Refreshs some values of old profile with new profile values.
    /// </summary>
    public void Migrate(SaveData toMigrate)
    {
        var newVersion = CreateNewProfile();

        newVersion.MigrateBoolProperties(toMigrate);
        newVersion.MigrateIntProperties(toMigrate);
        newVersion.MigrateFloatProperties(toMigrate);
        newVersion.MigrateStringProperties(toMigrate);
        newVersion.InitializeNewFields(toMigrate);
        newVersion.OverrideOldProfileProperties(toMigrate);
        toMigrate.IsMigrationChecking = false;
    }

    /// <summary>
    /// The data that can get saved or loaded for playing the game.
    /// Contains game progress of the ship.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        /// <summary>
        /// This flag is used to interrupt the recursive called methods on migration check.
        /// </summary>
        public bool IsMigrationChecking { get; set; }

        private Dictionary<E_PName, bool> BoolProps { get; set; } = new Dictionary<E_PName, bool>();
        private Dictionary<E_PName, int> IntProps { get; set; } = new Dictionary<E_PName, int>();
        private Dictionary<E_PName, float> FloatProps { get; set; } = new Dictionary<E_PName, float>();
        private Dictionary<E_PName, string> StringProps { get; set; } = new Dictionary<E_PName, string>();

        /// <summary>
        /// Creates a save data file with all currently default defined properties.
        /// </summary>
        public SaveData(List<PropSaver> _allProps)
        {
            foreach (var oneProp in _allProps)
            {
                switch (oneProp.type)
                {
                    case E_PType.E_Bool: BoolProps.Add(oneProp.name, false); break;
                    case E_PType.E_String: StringProps.Add(oneProp.name, string.Empty); break;
                    case E_PType.E_Int: IntProps.Add(oneProp.name, 0); break;
                    case E_PType.E_Float: FloatProps.Add(oneProp.name, 0f); break;
                }
            }
        }

        public void SetNewBoolProps(Dictionary<E_PName, bool> newProps) => BoolProps = newProps;
        public void SetNewIntProps(Dictionary<E_PName, int> newProps) => IntProps = newProps;
        public void SetNewFloatProps(Dictionary<E_PName, float> newProps) => FloatProps = newProps;
        public void SetNewStringProps(Dictionary<E_PName, string> newProps) => StringProps = newProps;

        public Dictionary<E_PName, bool> GetAllBoolProps() => BoolProps;
        public Dictionary<E_PName, int> GetAllIntProps() => IntProps;
        public Dictionary<E_PName, float> GetAllFloatProps() => FloatProps;
        public Dictionary<E_PName, string> GetAllStringProps() => StringProps;

        /// <summary>
        /// Returns null if the property is not found on this profile.
        /// </summary>
        public bool? GetBool(E_PName _propName)
        {
            if (BoolProps.TryGetValue(_propName, out bool _targetVal))
                return _targetVal;

            return null;
        }

        /// <summary>
        /// Returns null if the property is not found on this profile.
        /// </summary>
        public int? GetInt(E_PName _propName)
        {
            if (IntProps.TryGetValue(_propName, out int _targetVal))
                return _targetVal;

            return null;
        }

        /// <summary>
        /// Returns null if the property is not found on this profile.
        /// </summary>
        public float? GetFloat(E_PName _propName)
        {
            if (FloatProps.TryGetValue(_propName, out float _targetVal))
                return _targetVal;

            return null;
        }

        /// <summary>
        /// Returns null if the property is not found on this profile.
        /// </summary>
        public string GetString(E_PName _propName)
        {
            if (StringProps.TryGetValue(_propName, out string _targetVal))
                return _targetVal;

            return null;
        }

        public void SetBool(E_PName _propName, bool value) => BoolProps[_propName] = value;

        public void SetInt(E_PName _propName, int value) => IntProps[_propName] = value;

        public void SetFloat(E_PName _propName, float value) => FloatProps[_propName] = value;

        public void SetString(E_PName _propName, string value) => StringProps[_propName] = value;

        public bool IsBoolPropExisting(E_PName _propName) => BoolProps.ContainsKey(_propName);

        public bool IsIntPropExisting(E_PName _propName) => IntProps.ContainsKey(_propName);

        public bool IsFloatPropExisting(E_PName _propName) => FloatProps.ContainsKey(_propName);

        public bool IsStringPropExisting(E_PName _propName) => StringProps.ContainsKey(_propName);

        public bool AreBoolPropertiesDifferent(SaveData toCheck)
        {
            var isDifferent = false;

            foreach (var prop in BoolProps)
            {
                if (!toCheck.IsBoolPropExisting(prop.Key))
                    return true;
            }

            if (IsMigrationChecking)
                return false;

            if (toCheck.AreBoolPropertiesDifferent(this))
                return true;

            return isDifferent;
        }

        public bool AreIntPropertiesDifferent(SaveData toCheck)
        {
            var isDifferent = false;

            foreach (var prop in IntProps)
            {
                if (!toCheck.IsIntPropExisting(prop.Key))
                    return true;
            }

            if (IsMigrationChecking)
                return false;

            if (toCheck.AreIntPropertiesDifferent(this))
                return true;

            return isDifferent;
        }

        public bool AreFloatPropertiesDifferent(SaveData toCheck)
        {
            var isDifferent = false;

            foreach (var prop in FloatProps)
            {
                if (!toCheck.IsFloatPropExisting(prop.Key))
                    return true;
            }

            if (IsMigrationChecking)
                return false;

            if (toCheck.AreFloatPropertiesDifferent(this))
                return true;

            return isDifferent;
        }

        public bool AreStringPropertiesDifferent(SaveData toCheck)
        {
            var isDifferent = false;

            foreach (var prop in StringProps)
            {
                if (!toCheck.IsStringPropExisting(prop.Key))
                    return true;
            }

            if (IsMigrationChecking)
                return false;

            if (toCheck.AreStringPropertiesDifferent(this))
                return true;

            return isDifferent;
        }

        public void MigrateBoolProperties(SaveData toMigrate)
        {
            if (!AreBoolPropertiesDifferent(toMigrate))
                return;

            // 1. Check all props of toMigrate, if they exist on this (newProfile) and delete them if needed --- REMOVE PROPS NOT NEEDED ANYMORE
            var oldPropList = toMigrate.GetAllBoolProps();
            List<E_PName> toDelete = new List<E_PName>();
            foreach (var prop in oldPropList)
            {
                if (GetBool(prop.Key) == null)
                    toDelete.Add(prop.Key);
            }
            // Delete non used properties
            foreach (var key in toDelete)
            {
                oldPropList.Remove(key);
            }
            // 2. Check all props of newProfile, if they exist on toMigrate and add them if needed --- ADD NEW PROPS TO PROFILE WITH NEW DEFAULT VALUE
            foreach (var prop in BoolProps)
            {
                if (!oldPropList.ContainsKey(prop.Key))
                    oldPropList.Add(prop.Key, prop.Value);
            }
            // 3. Set new properties toMigrate
            toMigrate.SetNewBoolProps(oldPropList);
            // 4. Final check for similarity
            if (AreBoolPropertiesDifferent(toMigrate))
                throw new Exception();
        }

        public void MigrateIntProperties(SaveData toMigrate)
        {
            if (!AreIntPropertiesDifferent(toMigrate))
                return;

            // 1. Check all props of toMigrate, if they exist on this (newProfile) and delete them if needed --- REMOVE PROPS NOT NEEDED ANYMORE
            var oldPropList = toMigrate.GetAllIntProps();
            List<E_PName> toDelete = new List<E_PName>();
            foreach (var prop in oldPropList)
            {
                if (GetInt(prop.Key) == null)
                    toDelete.Add(prop.Key);
            }
            // Delete non used properties
            foreach (var key in toDelete)
            {
                oldPropList.Remove(key);
            }
            // 2. Check all props of newProfile, if they exist on toMigrate and add them if needed --- ADD NEW PROPS TO PROFILE WITH NEW DEFAULT VALUE
            foreach (var prop in IntProps)
            {
                if (!oldPropList.ContainsKey(prop.Key))
                    oldPropList.Add(prop.Key, prop.Value);
            }
            // 3. Set new properties toMigrate
            toMigrate.SetNewIntProps(oldPropList);
            // 4. Final check for similarity
            if (AreIntPropertiesDifferent(toMigrate))
                throw new Exception();
        }

        public void MigrateFloatProperties(SaveData toMigrate)
        {
            if (!AreFloatPropertiesDifferent(toMigrate))
                return;

            // 1. Check all props of toMigrate, if they exist on this (newProfile) and delete them if needed --- REMOVE PROPS NOT NEEDED ANYMORE
            var oldPropList = toMigrate.GetAllFloatProps();
            List<E_PName> toDelete = new List<E_PName>();
            foreach (var prop in oldPropList)
            {
                if (GetFloat(prop.Key) == null)
                    toDelete.Add(prop.Key);
            }
            // Delete non used properties
            foreach (var key in toDelete)
            {
                oldPropList.Remove(key);
            }
            // 2. Check all props of newProfile, if they exist on toMigrate and add them if needed --- ADD NEW PROPS TO PROFILE WITH NEW DEFAULT VALUE
            foreach (var prop in FloatProps)
            {
                if (!oldPropList.ContainsKey(prop.Key))
                    oldPropList.Add(prop.Key, prop.Value);
            }
            // 3. Set new properties toMigrate
            toMigrate.SetNewFloatProps(oldPropList);
            // 4. Final check for similarity
            if (AreFloatPropertiesDifferent(toMigrate))
                throw new Exception();
        }

        public void MigrateStringProperties(SaveData toMigrate)
        {
            if (!AreStringPropertiesDifferent(toMigrate))
                return;

            // 1. Check all props of toMigrate, if they exist on this (newProfile) and delete them if needed --- REMOVE PROPS NOT NEEDED ANYMORE
            var oldPropList = toMigrate.GetAllStringProps();
            List<E_PName> toDelete = new List<E_PName>();
            foreach (var prop in oldPropList)
            {
                if (GetString(prop.Key) == null)
                    toDelete.Add(prop.Key);
            }
            // Delete non used properties
            foreach (var key in toDelete)
            {
                oldPropList.Remove(key);
            }
            // 2. Check all props of newProfile, if they exist on toMigrate and add them if needed --- ADD NEW PROPS TO PROFILE WITH NEW DEFAULT VALUE
            foreach (var prop in StringProps)
            {
                if (!oldPropList.ContainsKey(prop.Key))
                    oldPropList.Add(prop.Key, prop.Value);
            }
            // 3. Set new properties toMigrate
            toMigrate.SetNewStringProps(oldPropList);
            // 4. Final check for similarity
            if (AreStringPropertiesDifferent(toMigrate))
                throw new Exception();
        }

        /// <summary>
        /// If you add new lists, arrays or dictionaries, they are null on the players old profile.
        /// The migration creates them new.
        /// </summary>
        public void InitializeNewFields(SaveData toMigrate)
        {
            
        }

        /// <summary>
        /// The properties listed in this method will override the old profile property values with the selected
        /// new profile property values, on migration.
        /// </summary>
        public void OverrideOldProfileProperties(SaveData toMigrate)
        {
            toMigrate.SetString(E_PName.ProfileVersion, GetString(E_PName.ProfileVersion));
        }

        public bool IsSameVersion(SaveData toCheck) => GetString(E_PName.ProfileVersion).Equals(toCheck.GetString(E_PName.ProfileVersion));

        /// <summary>
        /// Adds a collection data to profile.
        /// </summary>
        public void ReceiveNewCollectionData()
        {

        }
    }

    /// <summary>
    /// Defines a property that can be used in the profile system.
    /// </summary>
    public class PropSaver
    {
        public E_PType type { get; set; }
        public E_PName name { get; set; }

        /// <summary>
        /// Create a new property for profile
        /// </summary>
        public PropSaver(E_PType _type, E_PName _name)
        {
            type = _type;
            name = _name;
        }

        public override string ToString() => $"{name}";
    }
}