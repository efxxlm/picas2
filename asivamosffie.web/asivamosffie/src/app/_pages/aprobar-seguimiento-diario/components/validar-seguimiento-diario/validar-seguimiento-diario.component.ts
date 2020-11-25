import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-validar-seguimiento-diario',
  templateUrl: './validar-seguimiento-diario.component.html',
  styleUrls: ['./validar-seguimiento-diario.component.scss']
})
export class ValidarSeguimientoDiarioComponent implements OnInit {

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
