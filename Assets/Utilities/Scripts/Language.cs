using System;
using UnityEngine;

namespace Tetr4lab.UnityEngine {
    /// <summary>論理値許容型SystemLanguage</summary>
    public struct Language : IEquatable<Language> {
		private bool hasValue;
		private SystemLanguage language;
        /// <summary></summary>
		public static readonly Language Undef = new Language (false);
        /// <summary></summary>
        /// <param name="_hasValue"></param>
		public Language (bool _hasValue) {
			hasValue = _hasValue;
			language = _hasValue ? Application.systemLanguage : SystemLanguage.Unknown;
		}
        /// <summary></summary>
        /// <param name="_language"></param>
		public Language (SystemLanguage _language) {
			hasValue = _language != SystemLanguage.Unknown;
			language = _language;
		}
        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="language"></param>
        /// <returns></returns>
		public static bool TryParse (string name, bool ignoreCase, out Language language) {
			if (string.IsNullOrEmpty (name) || !Enum.TryParse (name, ignoreCase, out SystemLanguage syslang)) {
				language = Undef;
				return false;
			}
			language = syslang;
			return true;
		}
        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <returns></returns>
		public static bool TryParse (string name, out Language language) => TryParse (name, false, out language);
        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
		public static Language Parse (string name, bool ignoreCase) { TryParse (name, ignoreCase, out var language); return language; }
        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public static Language Parse (string name) { TryParse (name, false, out var language); return language; }
        /// <summary></summary>
        /// <returns></returns>
		public SystemLanguage GetValueOrDefault () => hasValue ? language : SystemLanguage.Unknown;
        /// <summary></summary>
        /// <param name="l"></param>
        /// <returns></returns>
		public SystemLanguage GetValueOrDefault (Language l) => hasValue ? language : (SystemLanguage) l;
        /// <summary></summary>
        /// <param name="l"></param>
		public static implicit operator bool (Language l) => l.hasValue;
        /// <summary></summary>
        /// <param name="l"></param>
		public static implicit operator SystemLanguage (Language l) => l.hasValue ? l.language : SystemLanguage.Unknown;
        /// <summary></summary>
        /// <param name="b"></param>
		public static implicit operator Language (bool b) => new Language (b);
        /// <summary></summary>
        /// <param name="l"></param>
		public static implicit operator Language (SystemLanguage l) => new Language (l);
        /// <inheritdoc/>
		public override string ToString () => hasValue ? language.ToString () : "Undef";
        /// <summary></summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
		public static bool operator == (Language a, Language b) => (a.hasValue == b.hasValue) && (!a.hasValue || a.language == b.language);
        /// <summary></summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
		public static bool operator != (Language a, Language b) => !(a == b);
        /// <summary></summary>
        /// <param name="other"></param>
        /// <returns></returns>
		public bool Equals (Language other) => (hasValue == other.hasValue) && (language == other.language);
        /// <inheritdoc/>
		public override bool Equals (object obj) => (obj == null || GetType () != obj.GetType ()) ? false : Equals ((Language) obj);
        /// <inheritdoc/>
		public override int GetHashCode () => hasValue ? language.GetHashCode () : int.MinValue;
	}

}
