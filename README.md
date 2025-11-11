## General Information

### Font
- Accepts `.ttf` or `.otf` font files.

### Glyphs  
- One or more characters to render.  
  - **Single character**: `"A"`  
  - **Multiple characters**: `"ABC123"`

> **Note:** Each character in a multi-character string is rendered as a separate image file. Any Unicode character can be used as a glyph and is saved as **Glyph_{UnicodeLabel}** (e.g., **Glyph_U+4E2D** for `中`). 

### Color
- Can be specified using a named color (e.g., `Red`) or a hexadecimal code (e.g., `#FF0000`).

### Formats 
- Output image format(s).  
  - **Single format**: `png`  
  - **Multiple formats**: `png,jpeg,bmp`  
  - **Supported formats**: `png`, `jpeg`, `webp`, `tiff`, `ico`, `avif`, `bmp`, `psd`, `pcx`, `tga`, `pnm`
    
### Background
- Transparent by default.
- When an output format does not support alpha, the image is automatically flattened to a white background.
- Formats without transparency include: `jpeg`, `avif`, `pcx`, `pnm`

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
| `--format <format>`  | Output image format(s).                        | `png`   |

---

### Examples

Render a single glyph `A` as a **Red JPEG**:

```
GlyphRasterizer.exe C:\Users\Alice\Fonts\Arial.ttf A C:\Users\Alice\Output --color Red --format jpeg
```

Render multiple glyphs `ABC123` **without additional options**:

```
GlyphRasterizer.exe C:\Users\Alice\Fonts\Arial.ttf ABC123 C:\Users\Alice\Output
```
> **Note:** Defaults to **Black PNG**.

---

## Interactive Console Tool

Guides you through configuration and rendering without passing command-line arguments.

### Overview

When you run the app without arguments:

```
GlyphRasterizer.exe
```

the tool starts an interactive session that:

1. Prompts you step-by-step for the required inputs (font, glyphs, formats, color, output directory).
2. Renders and saves the specified glyphs using your chosen settings.
3. Displays confirmation and error messages.
4. Offers options to:

   * **Restart with previous settings**; quickly re-run with the same font and output directory.
   * **Restart from scratch**; discard previous input and start over.

### Commands

During an interactive session, you can enter the following commands at any prompt:

| Command     | Description                                   |                            
| ----------- | --------------------------------------------- |
| **Back**    | Returns to the previous prompt.               |
| **Restart** | Restarts the session from the beginning.      |
| **Quit**    | Exits the program immediately.                |

### Example Session

<img width="1712" height="947" alt="Screenshot 2025-11-09 032109" src="https://github.com/user-attachments/assets/d40f711d-f053-40b5-97ca-e060b1367da4" />
