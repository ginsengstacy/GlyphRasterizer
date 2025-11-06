## General Information

**Font**  
- Accepts `.ttf` or `.otf` font files.

**Glyphs**  
- One or more characters to render.  
  - **Single character**: `"A"`  
  - **Multiple characters**: `"ABC123"`

> **Note:** Each character in a multi-character string is rendered as a separate image file. Any Unicode character can be used as a glyph and is saved as **Glyph_{UnicodeLabel}** (e.g., **Glyph_U+0041** for `A`). 

**Color**  
- Can be specified using a named color (e.g., `Red`) or a hexadecimal code (e.g., `#FF0000`).

**Image Size**  
- Square image size in pixels between `16` and `2048`.

**Formats**  
- Output image format(s).  
  - **Single format**: `png`  
  - **Multiple formats**: `png,jpeg,bmp`  
  - **Supported formats**: `png`, `jpeg`, `webp`, `tiff`, `ico`, `avif`, `bmp`, `psd`, `pcx`, `tga`, `pnm`

---

## Command Line Interface (CLI)

### Usage

```
GlyphRasterizer.exe <font> <glyph> <output> [options]
```

### Arguments

| Argument      | Description                                  |
| ------------- | -------------------------------------------- |
| `<font>`      | Path to the font file.                       |
| `<glyph>`    	| Glyph(s) to render.                          |
| `<output>`    | Output directory.                            |

### Options

| Option               | Description                                    | Default |
| -------------------- | ---------------------------------------------- | ------- |
| `--color <color>`    | Color of the rendered glyph(s).                | `Black` |
| `--size <int>`       | Square image size in pixels.                   | `256`   |
| `--format <format>`  | Output image format(s).                        | `png`   |

---

### Examples

Render a single character `A` as a **512×512 red JPEG**:

```
GlyphRasterizer.exe C:\Users\Alice\Fonts\Arial.ttf A C:\Users\Alice\Output --color Red --size 512 --format jpeg
```

Render multiple glyphs `ABC123` **without additional options**:

```
GlyphRasterizer.exe C:\Users\Alice\Fonts\Arial.ttf ABC123 C:\Users\Alice\Output
```
> **Note:** Defaults to **Black**, **256x256** and **PNG**.

---

## Interactive Console Tool

The interactive console mode guides you through configuration and rendering without passing command-line arguments.

### Overview

When you run the application without arguments:

```
GlyphRasterizer.exe
```

the tool starts an interactive session that:

1. Prompts you step-by-step for the required inputs (font, glyphs, formats, size, color, output directory).
2. Renders and saves the specified glyphs using your chosen settings.
3. Displays interactive confirmation and error messages.
4. Offers options to:

   * **Restart with previous settings** — quickly re-run with the same font and output directory.
   * **Restart from scratch** — discard previous input and start over.

### Commands

During an interactive session, you can enter the following commands at any prompt to control the session flow:

| Command     | Description                                   |                            
| ----------- | --------------------------------------------- |
| **Back**    | Returns to the previous prompt.               |
| **Restart** | Restarts the session from the beginning.      |
| **Quit**    | Exits the program immediately.                |

### Example Session

<img width="1642" height="988" alt="ExampleGlyphRasterizerSession" src="https://github.com/user-attachments/assets/a081180e-8bc1-4e21-8873-9e3a77eda463" />
