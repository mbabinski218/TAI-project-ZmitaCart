import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { environment } from '@env/environment';

@Component({
  selector: 'pp-app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './app.component.html',
})
export class AppComponent {

  constructor(
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer,
  ) {
    customIcons.forEach(([iconName, icon]) => {
      iconRegistry.addSvgIcon(
        iconName,
        sanitizer.bypassSecurityTrustResourceUrl(`${environment.iconPath}${icon}`),
      );
    });
  }
}

const customIcons: [string, string][] = [
  ["allegro", "allegro.svg"],
  ["add", "add.svg"],
  ["arrow", "arrow.svg"],
  ["logs", "logs.svg"],
  ["heart", "heart.svg"],
  ["heart-filled", "heart-filled.svg"],
  ["user", "user.svg"],
  ["menu", "menu.svg"],
  ["find", "find.svg"],
  ["android", "android.svg"],
  ["apple", "apple.svg"],
  ["my-camera", "camera-add.svg"],
  ["logout", "logout.svg"],
  ["user-basic", "user-basic.svg"],
  ["ZmitaCart", "ZmitaCart.svg"],
];
