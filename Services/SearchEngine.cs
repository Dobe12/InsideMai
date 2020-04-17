using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Threading.Tasks;
using InsideMai.Data;
using InsideMai.Models;

namespace InsideMai.Services
{
    public class SearchEngine
    {
        private readonly InsideMaiContext _context;

        public SearchEngine(InsideMaiContext context)
        {
            _context = context;
        }

        public List<Post> SearchPosts(List<Post> posts, string terms)
        {
            var result = posts.Select(p =>
                {
                    var countFoundedTitleTerms = p.Title.SpellOut()
                        .Distinct()
                        .Count(c => terms.SpellOut().Contains(c));
                    var countFoundedContentTerms = p.Content.SpellOut()
                        .Distinct()
                        .Count(c => terms.SpellOut().Contains(c));

                    return new
                    {
                        Value = p,
                        CountFoundedTerms = countFoundedTitleTerms > countFoundedContentTerms ?
                            countFoundedTitleTerms : countFoundedContentTerms,
                    };
                }).OrderByDescending(p => p.CountFoundedTerms)
                .Where(p => p.CountFoundedTerms > 0)
                .Select(p => p.Value)
                .ToList();

            return result;
        }

    }

    public static class StringExtension
    {
        public static string[] SpellOut(this string str)
        {
            return str.Split(' ');
        }
    }  

}
