using UnityEngine;
using System.Collections;
using Demiurg;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
    [System.Serializable]
    public class Tag
    {
        //static SlotComponentsProvider provider = new SlotComponentsProvider ();
        [SerializeField]
        string
            name;

        public string Name { get { return name; } }

        public int ID { get; internal set; }

        ITable criteria;
        ICallback checkFunction;

        public Tag (string name, int id, ICallback checkFunction, ITable criteria)
        {
            this.name = name;
            ID = id;
            this.checkFunction = checkFunction;
            this.criteria = criteria;
        }

        public bool CheckSlot (GameObject go)
        {
            //provider.SlotGO = go;
            //return (bool)checkFunction.Call (provider, criteria);
            return true;
        }
    }

    [System.Serializable]
    public class TagsCollection
    {
        public delegate void TagDelegate (Tag tag);

        public event TagDelegate TagAdded;
        public event TagDelegate TagRemoved;

        [SerializeField]
        List<Tag>
            assignedTags;


        public TagsCollection ()
        {
            assignedTags = new List<Tag> ();
            TagAdded += x =>
            {
            };
            TagRemoved += x =>
            {
            };
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
                if (assignedTags [0].ID < tag.ID && assignedTags.Count == 1)
                {
                    TagAdded (tag);
                    assignedTags.Insert (0, tag);
                }
                else
                {
                    for (int i = 1; i < assignedTags.Count; i++)
                        if (assignedTags [i].ID > tag.ID)
                        {
                            if (assignedTags [i - 1].ID != tag.ID)
                            {
                                TagAdded (tag);
                                assignedTags.Insert (i, tag);
                            }

                            return;
                        }
                    assignedTags.Add (tag);
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
                while (tag.ID != tags [i].ID);
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

        public int ComputeSimilarity (TagsCollection collection, Dictionary<Tag, int> weights)
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
                        int weight = 0;
                        weights.TryGetValue (tag, out weight);
                        similarity += weight;
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
}


