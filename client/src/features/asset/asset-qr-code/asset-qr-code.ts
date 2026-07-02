import { Component, Input, OnChanges, signal } from '@angular/core';
import QRCode from 'qrcode';

@Component({
  selector: 'app-asset-qr-code',
  imports: [],
  templateUrl: './asset-qr-code.html',
})
export class AssetQrCode implements OnChanges {
  @Input({ required: true }) assetId!: string;
  @Input() assetName = '';

  isOpen    = signal(false);
  qrDataUrl = signal('');
  error     = signal('');

  get qrContent(): string {
    return `${window.location.origin}/assets/${this.assetId}`;
  }

  async ngOnChanges() {
    if (!this.assetId) return;
    try {
      const url = await QRCode.toDataURL(this.qrContent, {
        width: 240,
        margin: 2,
        color: { dark: '#000000', light: '#ffffff' },
      });
      this.qrDataUrl.set(url);
    } catch {
      this.error.set('Σφάλμα δημιουργίας QR code.');
    }
  }

  toggle() {
    this.isOpen.update(v => !v);
  }

  download() {
    const a = document.createElement('a');
    a.download = `qr-${this.assetName || this.assetId}.png`;
    a.href = this.qrDataUrl();
    a.click();
  }

  print() {
    const win = window.open('', '_blank');
    if (!win) return;
    win.document.write(`<!DOCTYPE html><html><head>
      <title>QR – ${this.assetName}</title>
      <style>
        body { margin: 0; display: flex; flex-direction: column; align-items: center;
               justify-content: center; min-height: 100vh; font-family: sans-serif; }
        img  { width: 240px; height: 240px; }
        .name { margin-top: 10px; font-size: 16px; font-weight: 600; }
        .id   { margin-top: 4px; font-size: 10px; color: #666; font-family: monospace; }
      </style>
      </head><body>
        <img src="${this.qrDataUrl()}" />
        <p class="name">${this.assetName}</p>
        <p class="id">${this.assetId}</p>
      </body></html>`);
    win.document.close();
    win.focus();
    win.print();
    win.close();
  }
}
