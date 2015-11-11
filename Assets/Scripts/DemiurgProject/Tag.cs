using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tag
{
    public string Name { get; internal set; }
    public int ID { get; internal set; }
    public Tag (string name, int id)
    {
        Name = name;
        ID = id;
    }
}


public class TagsCollection
{
    public delegate void TagDelegate (Tag tag);
    public event TagDelegate TagAdded;
    public event TagDelegate TagRemoved;
    List<Tag> assignedTags;
    void Awake ()
    {
        assignedTags = new List<Tag> ();
    }
    public void AddTag (Tag tag)
    {
        if (assignedTags.Count == 0)
        {
            TagAdded (tag);
            assignedTags.Add (tag);
        }
        else
        {
            if (assignedTags [0].ID > tag.ID)
            {
                TagAdded (tag);
                assignedTags.Insert (0, tag);
            }
            else
                for (int i = 1; i < assignedTags.Count; i++)
                    if (assignedTags [i].ID > tag.ID)
                    {
                        if (assignedTags [i - 1].ID != tag.ID)
                        {
                            TagAdded (tag);
                            assignedTags.Insert (i, tag);
                        }
                        
                        break;
                    }
        }
    }
    
    public void RemoveTag (Tag tag)
    {
        int index = assignedTags.BinarySearch (tag, Comparator.Instance);
        if (index >= 0)
        {
            assignedTags.RemoveAt (index);
            TagRemoved (tag);
        }
    }
    
    public bool CheckTag (Tag tag)
    {
        return assignedTags.BinarySearch (tag, Comparator.Instance) >= 0;
    }
    
    public bool CheckTags (List<Tag> tags)
    {
        int tagIndex = 0;
        for (int i = 0; i < tags.Count; i++)
        {
            Tag tag = null;
            do
            {
                if (tagIndex == assignedTags.Count)
                    return false;
                tag = assignedTags [tagIndex++];
            }
            while (tag.ID != tags[i].ID);
        }
        return true;
    }
    public IEnumerable Tags ()
    {
        return assignedTags.AsReadOnly ();
    }
    public int TagsCount ()
    {
        return assignedTags.Count;
    }
    public int ComputeSimilarity (TagsCollection collection)
    {
        int similarity = 0;
        IEnumerable<Tag> otherTags = collection.Tags () as IEnumerable<Tag>;
        int tagIndex = 0;
        foreach (var tag in otherTags)
        {
            for (int t = tagIndex; t < assignedTags.Count; t++)
                if (assignedTags [t].ID == tag.ID)
                {
                    tagIndex = t;
                    similarity++;
                    break;
                }
                else
                if (assignedTags [t].ID > tag.ID)
                {
                    tagIndex = t;
                    break;
                }
        }
        return similarity;
    }

    class Comparator : Comparer<Tag>
    {
        public static Comparator Instance = new Comparator ();
        public override int Compare (Tag x, Tag y)
        {
            return x.ID.CompareTo (y.ID);
        }


    }
}