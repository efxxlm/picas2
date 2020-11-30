import { Component, OnInit } from '@angular/core';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private authe: AutenticacionService) {
    this.actualUser = this.authe.actualUser;
  }

  actualUser: any;
  roles :string="";

  ngOnInit(): void {
    this.authe.actualUser$.subscribe(user => {
      this.roles="";
      this.actualUser = user;     
      if(this.actualUser)
      {        
        this.actualUser.rol.forEach(element => {
          this.roles+=element.perfil.nombre+" ";
        });
      }      
    });
  }

}
