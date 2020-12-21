# GBlason

## Introduction

This repository is associated with the namesake project [GBlason](https://github.com/users/grathad/projects/1). The name is using the French version of the term `Blason` rather than the English one for historical reason (most of the vocabulary used in blazonning comes from French)

> The word "Blazon" is used with some number of meanings, but practically it may be confined to the verb "to blazon," which is to describe in words a given coat of arms, and the noun "blazon," which is such a description.

The project is separated in multiple concept. The central one is the definition of a file format, that can represent any kind of coat of arms. The foundation of the format is based on the logic of the construct of blazon, the documentation on the subject is relatively scarce, and mostly done before today's notions of grammar or syntax. Most of the inspiration comes from [there](https://www.gutenberg.org/files/41617/41617-h/41617-h.htm)

## Format

The notion of format should be redundant, in the sense that there is a defined (although not respected) way of describing a coat of arms using plain langugage. Given the complexity of some coat of arms, the goal is still to represent those blazon using a more modern definition of the different parts that composes it.
The format is thus a center piece of this project, used as an input for rendering purpose, or as an output from parsing a bazon.

## Parser

The parser is a logic that use a grammar definition and use it to interpret (parse) a blazon. Its goal is to turn regular words into their associated hearaldry concepts, especially in relation to each other.
The Parser itself is made of multiple blocks:
1. Pilot: The pilot is the part of the parser that do execute the parsing using the grammar and the blazon text
1. Grammar: The definition of rules that are applied by the parser to understand a blazon, it is language based
1. Keywords: The specific vocabulary used by the pilot when parsing

## Renderer

A distant project, meant to represent the format into a SVG

## Editor

An even more distant project, meant to update the format through UI and thus enable the edition of the rendered result
