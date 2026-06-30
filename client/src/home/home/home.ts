import { Component, inject } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { Nav } from "../../layout/nav/nav";

@Component({
  selector: 'app-home',
  imports: [Nav],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  accountService=inject(AccountService);
  

}
