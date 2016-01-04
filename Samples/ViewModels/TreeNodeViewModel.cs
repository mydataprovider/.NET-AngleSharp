﻿using AngleSharp.DOM;
using AngleSharp.DOM.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Samples.ViewModels
{
    public class TreeNodeViewModel : BaseViewModel
    {
        ObservableCollection<TreeNodeViewModel> children;
        Boolean expanded;
        Boolean selected;
        String value;
        Brush foreground;
        TreeNodeViewModel expansionElement;
        ObservableCollection<TreeNodeViewModel> parent;

        [ThreadStatic]
        static StringBuilder buffer;

        TreeNodeViewModel()
        {
            children = new ObservableCollection<TreeNodeViewModel>();
            expanded = false;
            selected = false;
            foreground = Brushes.Black;
        }

        public Boolean IsExpanded
        {
            get { return expanded; }
            set
            {
                expanded = value;
                RaisePropertyChanged();

                if (expansionElement != null && parent != null)
                {
                    if (value)
                        parent.Insert(parent.IndexOf(this) + 1, expansionElement);
                    else
                        parent.Remove(expansionElement);
                }
            }
        }

        public Brush Foreground
        {
            get { return foreground; }
            set { foreground = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TreeNodeViewModel> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Boolean IsSelected
        {
            get { return selected; }
            set
            {
                selected = value;
                RaisePropertyChanged();
            }
        }

        public String Value
        {
            get { return value; }
            set
            {
                this.value = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TreeNodeViewModel> Children
        {
            get { return children; }
        }

        public static TreeNodeViewModel Create(Node node)
        {
            if (node is TextNode)
                return Create((TextNode)node);
            else if (node is Comment)
                return new TreeNodeViewModel { Value = Comment(((Comment)node).Data), Foreground = Brushes.Gray };
            else if (node is DocumentType)
                return new TreeNodeViewModel { Value = node.ToHtml(), Foreground = Brushes.DarkGray };
            else if(node is Element)
                return Create((Element)node);

            return null;
        }

        static TreeNodeViewModel Create(TextNode text)
        {
            if(String.IsNullOrEmpty(text.Data))
                return null;

            var data = text.Data.Split(ws, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length == 0)
                return null;

            return new TreeNodeViewModel { Value = String.Join(" ", data), Foreground = Brushes.SteelBlue };
        }

        static TreeNodeViewModel Create(Element node)
        {
            var vm = new TreeNodeViewModel { Value = OpenTag(node) };

            foreach (var element in SelectFrom(node.ChildNodes))
            {
                element.parent = vm.children;
                vm.children.Add(element);
            }

            if (vm.children.Count != 0)
                vm.expansionElement = new TreeNodeViewModel { Value = CloseTag(node) };

            return vm;
        }

        public static IEnumerable<TreeNodeViewModel> SelectFrom(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                TreeNodeViewModel element = Create(node);

                if (element != null)
                    yield return element;
            }
        }

        static StringBuilder StartString()
        {
            return buffer ?? (buffer = new StringBuilder());
        }

        static StringBuilder StartString(Char c)
        {
            return StartString().Append(c);
        }

        static StringBuilder StartString(String str)
        {
            return StartString().Append(str);
        }

        static String ReleaseString()
        {
            var s = buffer.ToString();
            buffer.Clear();
            return s;
        }

        static String Comment(String comment)
        {
            StartString("<!--").Append(comment).Append("-->");
            return ReleaseString();
        }

        static String OpenTag(Element element)
        {
            StartString('<').Append(element.TagName).Append(element.Attributes.ToHtml()).Append('>');
            return ReleaseString();
        }

        static String CloseTag(Element element)
        {
            StartString("</").Append(element.TagName).Append('>');
            return ReleaseString();
        }
    }
}
