## Introduction

The grammar engine is handling everything related to blazon (i.e text representing the coat of arms)

It is a translator between a text (blazon) and a [format](https://gitlab.com/gblason/Format) (see the format project)

### Text to format

The main purpose of the grammar engine is to turn a blazon into a valid format that can be used to render or save the resulting coat of arms

The contract works as follow:

* Input:
 * The text to read and translate as format
 * The plugins to use for the parsing
 
* Output:
 * The format generated from the text
 * The list of errors encountered while parsing
 
### Format to text

* Input:
 * Format
 
* Output:
 * Plugins to use for the creation of the text
 * List of text that are a valid representation of the format
 
## Plugins

The grammar engine support plugins that can be used to define the behavior of the parsing.
The plugin contract is defined and is meant to receive the text (string or stream) and expect as a result a format with the list of errors encountered.

The format definition have to be respected to produce a correct output.
Everything else is up to the implementation.

The default plugins that enable the grammar parser are included within the solution.
For now it is one default plugin per language supported.

## Blazon

> The word "Blazon" is used with some number of meanings, but practically it may be confined to the verb "to blazon," which is to describe in words a given coat of arms, and the noun "blazon," which is such a description.

The Grammar project, have more than the format to worry about, and need to implement the rules of blazonment in order to turn every day words into a valid format (or detect potential errors there).

### Order

> The commencement of any blazon is of necessity a description of the field

#### Field

* The colour is used first to represent a simple field
* For composite fields, the composition is the first terms

The only exception is for partition with only colours, in this case the colour is stated first, then the partition, then the colour.

#### Semy

After the field description, it is possible to (if present) describe any seme.

#### Charge

The second thing to be mentioned in the blazon is the principal charge.
If multiple charges, then the colour can be shared at the end if it is the same for all the charges.

If the charge is charged (possible for ordinary) then the secondary charge is of lesser importance and thus will always be defined (which is why there are "on" keywords)

The position of the charges need not be specified when they would naturally fall into a certain position with regard to the ordinaries. Thus, 

* A chevron between three figures of necessity has two in chief and one in base.
* A bend between two figures of necessity has one above and one below. A fess has two above and one below.
* A cross between four has one in each angle. 

In none of these cases is it necessary to state the position.
If, however, those positions or numbers do not come within the category mentioned, care must be taken to specify what the coat exactly is.
If a bend is accompanied only by one charge, the position of this charge must be stated.

##### Ordinary

