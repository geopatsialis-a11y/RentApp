import { Component, OnDestroy, AfterViewInit, ViewChild, ElementRef, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BrowserMultiFormatReader, IScannerControls } from '@zxing/browser';

@Component({
  selector: 'app-qr-scanner',
  imports: [RouterLink],
  templateUrl: './qr-scanner.html',
})
export class QrScanner implements AfterViewInit, OnDestroy {
  private router = inject(Router);

  @ViewChild('video') videoRef!: ElementRef<HTMLVideoElement>;

  scanning = signal(true);
  error    = signal('');
  detected = signal('');

  private controls?: IScannerControls;
  private reader   = new BrowserMultiFormatReader();

  async ngAfterViewInit() {
    await this.startCamera();
  }

  private async startCamera() {
    this.error.set('');
    this.detected.set('');
    this.scanning.set(true);
    try {
      const devices = await BrowserMultiFormatReader.listVideoInputDevices();
      // On mobile the rear camera is typically the last device
      const deviceId = devices.length > 1 ? devices[devices.length - 1].deviceId : undefined;

      this.controls = await this.reader.decodeFromVideoDevice(
        deviceId,
        this.videoRef.nativeElement,
        (result, _err) => {
          if (!result) return;
          const text = result.getText();
          this.detected.set(text);
          this.scanning.set(false);
          this.controls?.stop();

          // Match /assets/<UUID> anywhere in the string (full URL or bare path)
          const match = text.match(/\/assets\/([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})/i);
          if (match) {
            this.router.navigate(['/assets', match[1]]);
          }
        }
      );
    } catch (e: unknown) {
      const name = (e as DOMException)?.name;
      this.error.set(
        name === 'NotAllowedError'
          ? 'Δεν επιτράπηκε η πρόσβαση στην κάμερα. Ελέγξτε τα δικαιώματα του browser.'
          : 'Δεν ήταν δυνατή η εκκίνηση της κάμερας.'
      );
      this.scanning.set(false);
    }
  }

  restart() {
    this.controls?.stop();
    this.startCamera();
  }

  ngOnDestroy() {
    this.controls?.stop();
  }
}
