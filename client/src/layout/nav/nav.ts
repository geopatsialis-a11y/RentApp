import { Component, computed, HostListener, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-nav',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  accountService = inject(AccountService);
  sidebarOpen = signal(false);
  isAdmin = computed(() => (this.accountService.currentUser() as any)?.roles?.includes('Admin') ?? false);

  @HostListener('document:keydown.escape')
  onEsc() { this.sidebarOpen.set(false); }

  toggleSidebar() { this.sidebarOpen.update(v => !v); }
  closeSidebar() { this.sidebarOpen.set(false); }
  logout() { this.accountService.logout(); }
}
