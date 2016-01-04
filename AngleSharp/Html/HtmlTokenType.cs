﻿using System;

namespace AngleSharp.Html
{
    /// <summary>
    /// An enumation of all possible tokens.
    /// </summary>
    enum HtmlTokenType
    {
        /// <summary>
        /// The DOCTYPE token.
        /// </summary>
        DOCTYPE,
        /// <summary>
        /// The start tag token to mark open tags.
        /// </summary>
        StartTag,
        /// <summary>
        /// The end tag token to mark ending tags.
        /// </summary>
        EndTag,
        /// <summary>
        /// The comment tag to mark comments.
        /// </summary>
        Comment,
        /// <summary>
        /// The character token to mark a single character.
        /// </summary>
        Character,
        /// <summary>
        /// The characters token to mark a sequence of characters.
        /// </summary>
        Characters,
        /// <summary>
        /// The End-Of-File token to mark the end.
        /// </summary>
        EOF
    }
}
