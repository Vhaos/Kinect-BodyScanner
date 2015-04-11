<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- *** root ************************************************************************************* -->
  <xsl:template match="/">
    <html>
      <head>
      </head>
      <body BGCOLOR="F5F5F5" leftmargin="0" topmargin="0">
        <table width="640px" cellpadding="0" cellspacing="0" align="center">
          <tr>
            <td colspan="4" height="20">
              <p>  </p>
            </td>
          </tr>
          <tr>
            <td colspan="4" align="center" bgcolor="#F1F1F2">
              <font face="arial" size="2">
                <xsl:value-of select="/MSV_Measures/file/@name" />
              </font>
            </td>
          </tr>

          <tr>
            <td colspan="4" height="20">
              <p>  </p>
            </td>
          </tr>
          <tr>
            <td colspan="4" height="20">
              <p> Measurements </p>
            </td>
          </tr>

          <xsl:apply-templates select="/MSV_Measures/measure[@id='MSV_Height']">
            <xsl:with-param name="mName">Height</xsl:with-param>
          </xsl:apply-templates>
          <xsl:apply-templates select="/MSV_Measures/measure[@id='MSV_Chest']">
            <xsl:with-param name="mName">Chest / Bust</xsl:with-param>
            <xsl:with-param name="mColour">1</xsl:with-param>
          </xsl:apply-templates>
          <xsl:apply-templates select="/MSV_Measures/measure[@id='MSV_Waist']">
            <xsl:with-param name="mName">Waist</xsl:with-param>
          </xsl:apply-templates>
          <xsl:apply-templates select="/MSV_Measures/measure[@id='MSV_Hip']">
            <xsl:with-param name="mName">Hip</xsl:with-param>
            <xsl:with-param name="mColour">1</xsl:with-param>
          </xsl:apply-templates>
          <xsl:apply-templates select="/MSV_Measures/measure[@id='MSV_InsideLeg']">
            <xsl:with-param name="mName">Inside Leg</xsl:with-param>
          </xsl:apply-templates>

          <!--
          <tr>
            <td colspan="4" height="20">
              <p>  </p>
            </td>
          </tr>
          <tr>
            <td colspan="4" height="20">
              <p>Shape Classification </p>
            </td>
          </tr>
          <tr>
            <td bgcolor="#E0E0E0" width="5%" height="20"> </td>
            <td bgcolor="#E0E0E0" colspan="3" width="95%" align="left" valign="middle" class = "style2">
              <font face="arial" size="2">
                <xsl:variable name="lh" select="(/MSV_Measures/measure[@id='MSV_Hip'])"/>
                <xsl:variable name="w" select="(/MSV_Measures/measure[@id='MSV_Waist'])"/>
                <xsl:variable name="r" select="$w div $lh"/>
                <xsl:choose>
                  <xsl:when test="($r &gt; 0.860)">
                    Emerald
                  </xsl:when>
                  <xsl:when test="($r &gt; 0.779) and ($r &lt; 0.860)">
                    Sapphire
                  </xsl:when>
                  <xsl:when test="($r &lt; 0.779)">
                    Ruby
                  </xsl:when>
                  <xsl:otherwise>
                    Failed
                  </xsl:otherwise>
                </xsl:choose>
              </font>
            </td>
          </tr>
        -->
        </table>

      </body>
    </html>
  </xsl:template>

  <!-- *** measure ********************************************************************************* -->
  <xsl:template match="measure">
    <xsl:param name="mName" />
    <xsl:param name="mColour" />
    <tr>
      <xsl:choose>
        <xsl:when test="$mColour = ''">
          <td class="tabKey" bgcolor="#F0F0F0" width="5%"> </td>
          <td class="tabKey" bgcolor="#F0F0F0" width="45%">
            <font face="arial" size="2">
              <xsl:choose>
                <xsl:when test="$mName = ''">
                  <xsl:value-of select="@name" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$mName" />
                </xsl:otherwise>
              </xsl:choose>
            </font>
          </td>
          <td class="tabValue" align="right" bgcolor="#F0F0F0" width="45%">
            <font face="arial" size="2">
              <xsl:value-of select="format-number(.,'##0.0')" /><xsl:value-of select="@unit" />
              / <xsl:value-of select="format-number(. div 2.54,'##0.0')" /> in
            </font>
          </td>
          <td class="tabKey" bgcolor="#F0F0F0" width="5%"> </td>
        </xsl:when>
        <xsl:otherwise>
          <td class="tabKey" bgcolor="#E0E0E0" width="5%"> </td>
          <td class="tabKey" bgcolor="#E0E0E0" width="45%">
            <font face="arial" size="2">
              <xsl:choose>
                <xsl:when test="$mName = ''">
                  <xsl:value-of select="@name" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$mName" />
                </xsl:otherwise>
              </xsl:choose>
            </font>
          </td>
          <td class="tabValue" align="right" bgcolor="#E0E0E0" width="45%">
            <font face="arial" size="2">
              <xsl:value-of select="format-number(.,'##0.0')" /><xsl:value-of select="@unit" />
              / <xsl:value-of select="format-number(. div 2.54,'##0.0')" /> in
            </font>
          </td>
          <td class="tabKey" bgcolor="#E0E0E0" width="5%"> </td>
        </xsl:otherwise>
      </xsl:choose>
    </tr>
  </xsl:template>

</xsl:stylesheet>