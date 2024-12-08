Ripped using CTR-tools, created by DCxDemo. (https://github.com/CTR-tools/CTR-tools).

Available in both .dae and .obj with vertex color support, with the latter being the original export. The latest version of Blender (as of writing, 3.5.1) supports importing .obj with vertex colors, so I recommend doing that if possible.

As should be common knowledge, the PlayStation applies vertex colors differently from what is commonly done nowadays: instead of blending colors by multiplying each of the two colors being blended and dividing by 255, a division by 128 is performed instead. This causes vertex colors to have twice as much intensity as they do nowadays, such that #808080 becomes the neutral vertex color value as opposed to #FFFFFF, with the latter causing colors to become full white instead.

Crash Team Racing's textures use 15-bit color, which were transformed into 24-bit color by mapping the 15-bit values, which range from 0 to 31, onto a 0-255 range and rounding.

The textures are named following a convention that is used with CTR texture modding in mind.

Included with the .zip is a script for Blender ("Blender script for proper rendering of CTR models.py") that applies a correction to its renderer for displaying the vertex colors properly; it's not perfect, as there will still be noticeable seams between textured and texture-less* geometry, but it's probably the best that can be done accounting for everything above. In order to use it, you'll need to go to the scripting tab from the "General" default template, click Text -> Open, select the script file, check the first few lines of the script and do what it says (hint: the mesh name should be the one from the scene collection) then click Text -> Run Script. The colors may look washed out, you'll want to go to Render Properties -> Color Management and set View Transform to "Standard". It also automatically sets the materials to clamp the textures (named in Blender as "extend"), sets transparency and additive blending on the materials where needed, and removes texture filtering on the materials.

*Texture-less geometry uses a 1x1 texture with color 16 of a 5-bit color range, which is often used as the median color, being #848484 as mapped onto 8-bit values.