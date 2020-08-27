import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-revision-acta',
  templateUrl: './revision-acta.component.html',
  styleUrls: ['./revision-acta.component.scss']
})
export class RevisionActaComponent implements OnInit {

  acta;

  constructor ( private routes: Router ) {
    this.getActa();
    console.log( this.acta );
  }

  ngOnInit(): void {
  }

  getActa () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl ) {
      this.routes.navigateByUrl( '/compromisosActasComite' );
      return;
    };
    this.acta = this.routes.getCurrentNavigation().extras.state.acta;
  }

}
