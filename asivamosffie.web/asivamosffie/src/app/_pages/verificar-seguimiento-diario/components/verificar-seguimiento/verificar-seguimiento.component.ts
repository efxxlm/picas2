import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-verificar-seguimiento',
  templateUrl: './verificar-seguimiento.component.html',
  styleUrls: ['./verificar-seguimiento.component.scss']
})
export class VerificarSeguimientoComponent implements OnInit {

  seguimientoId: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.seguimientoId = params.id;
      console.log(this.seguimientoId);
    });
  }

}
