import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-registrar-ajuste-programacion',
  templateUrl: './registrar-ajuste-programacion.component.html',
  styleUrls: ['./registrar-ajuste-programacion.component.scss']
})
export class RegistrarAjusteProgramacionComponent implements OnInit {

  detalleId: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      console.log(this.detalleId);
    });
  }

}
