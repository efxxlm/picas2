import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-registrar-revision-juridica',
  templateUrl: './registrar-revision-juridica.component.html',
  styleUrls: ['./registrar-revision-juridica.component.scss']
})
export class RegistrarRevisionJuridicaComponent implements OnInit {

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
