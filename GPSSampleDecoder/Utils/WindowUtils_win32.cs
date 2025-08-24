/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.Utils
{

   public class WindowUtils
   {

      private static readonly Lazy<WindowUtils> lazy = new Lazy<WindowUtils>(() => new WindowUtils());
      public static WindowUtils Instance
      {
         get
         {
            return lazy.Value;
         }
      }

      public void ShowMessageBox(string message)
      {

      }

   }
}
