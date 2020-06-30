import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { UrlResolver } from '@angular/compiler';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  data: any[];

  constructor(private authe: AutenticacionService,private router: Router) {
    this.actualUser = this.authe.actualUser;
   }

  actualUser: any;
  ngOnInit(): void {
    this.authe.actualUser$.subscribe(user => { 
      if(user==null)
      {
        console.log("iniciando");
      }
      else{
        this.actualUser = user;         
        if(user.fechaUltimoIngreso==null || user.cambiarContrasena)
        {        
          this.router.navigate(['/cambiarContrasena']);
        }
      }      
    });
  }
  probarconsumo()
  {
    this.authe.consumoePrueba().subscribe(data=>{this.data=data});
  }

}
