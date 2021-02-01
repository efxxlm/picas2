import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-validar-ajuste',
  templateUrl: './validar-ajuste.component.html',
  styleUrls: ['./validar-ajuste.component.scss']
})
export class ValidarAjusteComponent implements OnInit {

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
