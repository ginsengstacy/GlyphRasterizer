## General Information

**Font**  
- Accepts `.ttf` or `.otf` font files.

**Glyphs**  
- One or more characters to render.  
  - **Single character**: `"A"`  
  - **Multiple characters**: `"ABC123"`

> **Note:** If multiple glyphs are entered, each one is rendered as a separate image file. Any valid Unicode sequence can be used as a glyph.

**Color**  
- Can be specified using a named color (e.g., `Red`) or a hexadecimal code (e.g., `#FF0000`).

**Image Size**  
- Square image size in pixels between `1` and `2048`.

**Formats**  
- Output image format(s).  
  - **Single format**: `png`  
  - **Multiple formats**: `png, jpeg, bmp`  
  - **Special value**: `all` renders all supported formats.  
  - **Supported formats**: `png`, `jpeg`, `bmp`, `tiff`, `ico`.

---

## Command Line Interface (CLI)

### Usage

```
GlyphRasterizer.exe <font> <glyphs> <output> [options]
```

### Arguments

| Argument      | Description                                  |
| ------------- | -------------------------------------------- |
| `<font>`      | Path to the font file.                       |
| `<glyphs>`    | Glyph(s) to render.                          |
| `<output>`    | Output directory.                            |

### Options

| Option               | Description                                    | Default |
| -------------------- | ---------------------------------------------- | ------- |
| `--color <color>`    | Color of the rendered glyph(s).                | `Black` |
| `--size <int>`       | Square image size in pixels. Range: 1–2048.    | `256`   |
| `--format <formats>` | Output image format(s).                        | `png`   |

---

### Examples

Render a single character `"A"` as a **512×512 red JPEG**:

```
GlyphRasterizer.exe "C:\Users\Alice\Fonts\Arial.ttf" "A" "C:\Users\Alice\Output" --color Red --size 512 --format jpeg
```

Render multiple glyphs `"ABC123"` as **all supported formats**:

```
GlyphRasterizer.exe "C:\Users\Alice\Fonts\Arial.ttf" "ABC123" "C:\Users\Alice\Output" --format all
```
> **Note:** Defaults to **black** and **PNG**.

---

## Interactive Console Tool

The interactive console mode guides you through configuration and rendering without passing command-line arguments.

### Overview

When you run the application without arguments:

```
GlyphRasterizer.exe
```

the tool starts an interactive session that:

1. Prompts you step-by-step for the required inputs (font, glyphs, color, size, formats, output directory).
2. Renders and saves the specified glyphs using your chosen settings.
3. Displays interactive confirmation and error messages.
4. Offers options to:

   * **Restart with previous settings** — quickly re-run with the same font and output directory.
   * **Restart from scratch** — discard previous input and start over.

### Commands

During an interactive session, you can enter the following commands at any prompt to control the session flow:

| Command     | Aliases             | Description                                                                 |
| ----------- | ------------------- | --------------------------------------------------------------------------- |
| **Back**    | `back`, `undo`      | Returns to the previous prompt.                                             |
| **Restart** | `restart`, `reload` | Restarts the session from the beginning.      |
| **Quit**    | `quit`, `exit`      | Exits the program immediately.                                              |

### Example Session

<img width="1056" height="445" alt="Screenshot 2025-10-31 063515" src="https://github.com/user-attachments/assets/b48a4bb4-b9b3-435b-947a-1640a1251d93" />

## Notes

* The tool asks to overwrite existing files in the output directory if names conflict.
